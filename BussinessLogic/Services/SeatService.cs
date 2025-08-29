using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelAndDto.Models;
using DataAccess.Repositories;
using BussinessLogic.Interfaces;


namespace BussinessLogic.Services
{
    public class SeatService : ISeatService
    {
        private readonly SeatRepository _seatRepo;

        public SeatService(SeatRepository seatRepo)
        {
            _seatRepo = seatRepo;
        }

        public void AddSeat(int flightId, int seatNumber)
        {
            try
            {
                var seat = new Seat
                {
                    FlightId = flightId,
                    SeatNumber = seatNumber,
                    IsAvailable = true
                };
                _seatRepo.Add(seat);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error adding seat - FlightId: {flightId}, SeatNumber: {seatNumber}, Error: {ex.Message}");
                throw;
            }
        }

        public Seat? GetSeat(int id)
        {
            try
            {
                return _seatRepo.GetById(id);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting seat with ID {id}: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Seat> GetAllSeats()
        {
            try
            {
                return _seatRepo.GetAll();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting all seats: {ex.Message}");
                throw;
            }
        }

        public void UpdateSeat(Seat seat)
        {
            try
            {
                _seatRepo.Update(seat);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating seat with ID {seat.Id}: {ex.Message}");
                throw;
            }
        }

        public void DeleteSeat(int id)
        {
            try
            {
                _seatRepo.Delete(id);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting seat with ID {id}: {ex.Message}");
                throw;
            }
        }
    }
}