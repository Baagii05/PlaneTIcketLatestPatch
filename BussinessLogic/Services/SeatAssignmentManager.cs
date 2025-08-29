using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BussinessLogic.Interfaces;
using ModelAndDto.Models;


namespace BussinessLogic.Services
{
    public class SeatAssignmentManager : ISeatAssignmentManager
    {
        private readonly ConcurrentQueue<SeatAssignmentRequest> _queue;
        private readonly ISeatService _seatService;
        private readonly IPassengerService _passengerService;
        private readonly CancellationTokenSource _cts;
        private readonly Task _processingTask;

        public SeatAssignmentManager(
            ISeatService seatService,
            IPassengerService passengerService)
        {
            try
            {
                _queue = new ConcurrentQueue<SeatAssignmentRequest>();
                _seatService = seatService;
                _passengerService = passengerService;
                _cts = new CancellationTokenSource();
                _processingTask = Task.Run(ProcessQueueAsync);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error initializing SeatAssignmentManager: {ex.Message}");
                throw;
            }
        }

        public void EnqueueRequest(SeatAssignmentRequest request)
        {
            try
            {
                _queue.Enqueue(request);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error enqueueing seat request: {ex.Message}");
                throw;
            }
        }

        public async Task<(bool Success, string Message)> AssignSeatAsync(int passengerId, int flightId, int seatNumber)
        {
            try
            {
                var tcs = new TaskCompletionSource<(bool Success, string Message)>();

                var request = new SeatAssignmentRequest
                {
                    PassengerId = passengerId,
                    FlightId = flightId,
                    SeatNumber = seatNumber,
                    Callback = (success, message) => tcs.SetResult((success, message))
                };

                EnqueueRequest(request);
                return await tcs.Task;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in AssignSeatAsync - PassengerId: {passengerId}, FlightId: {flightId}, SeatNumber: {seatNumber}, Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsSeatAvailableAsync(int flightId, int seatNumber)
        {
            try
            {
                var seats = _seatService.GetAllSeats();
                var seat = seats.FirstOrDefault(s => s.FlightId == flightId && s.SeatNumber == seatNumber);
                return seat != null && !seat.IsAvailable;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error checking seat availability - FlightId: {flightId}, SeatNumber: {seatNumber}, Error: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessQueueAsync()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    if (_queue.TryDequeue(out var request))
                    {
                        try
                        {
                            await ProcessSeatRequest(request);
                        }
                        catch (Exception ex)
                        {
                            request.Callback?.Invoke(false, $"Error processing seat request: {ex.Message}");
                            Console.Error.WriteLine($"Error processing seat request - PassengerId: {request.PassengerId}, FlightId: {request.FlightId}, SeatNumber: {request.SeatNumber}, Error: {ex.Message}");
                        }
                    }
                    else
                    {
                        await Task.Delay(50, _cts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Normal cancellation, break the loop
                    break;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in ProcessQueueAsync main loop: {ex.Message}");
                    // Continue processing other requests despite errors
                }
            }
        }

        private async Task ProcessSeatRequest(SeatAssignmentRequest request)
        {
            var seats = _seatService.GetAllSeats();
            var seat = seats.FirstOrDefault(s =>
                s.FlightId == request.FlightId &&
                s.SeatNumber == request.SeatNumber);

            if (seat == null)
            {
                request.Callback?.Invoke(false, "Seat not found.");
                return;
            }

            if (!seat.IsAvailable)
            {
                request.Callback?.Invoke(false, "Seat already reserved.");
                return;
            }

            try
            {
                // Update seat status
                seat.IsAvailable = false;
                _seatService.UpdateSeat(seat);

                // Update passenger seat assignment
                var passenger = _passengerService.GetPassenger(request.PassengerId);
                if (passenger != null)
                {
                    passenger.SeatId = seat.Id;
                    passenger.SeatNumber = seat.SeatNumber;
                    _passengerService.UpdatePassenger(passenger);
                    request.Callback?.Invoke(true, "Seat assigned successfully.");
                }
                else
                {
                    // Rollback seat status if passenger not found
                    seat.IsAvailable = true;
                    _seatService.UpdateSeat(seat);
                    request.Callback?.Invoke(false, "Passenger not found.");
                }
            }
            catch (Exception ex)
            {
                // Attempt rollback on error
                try
                {
                    seat.IsAvailable = true;
                    _seatService.UpdateSeat(seat);
                }
                catch (Exception rollbackEx)
                {
                    Console.Error.WriteLine($"Error during rollback: {rollbackEx.Message}");
                }
                throw; // Re-throw the original exception
            }
        }

        public void Dispose()
        {
            try
            {
                _cts.Cancel();
                _processingTask.Wait();
            }
            catch (AggregateException ex)
            {
                Console.Error.WriteLine($"Error during disposal: {ex.Message}");
            }
            finally
            {
                _cts.Dispose();
            }
        }
    }
}