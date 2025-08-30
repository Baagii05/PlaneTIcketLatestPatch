using System;
using System.Drawing;
using System.Drawing.Printing;
using ModelAndDto.Models;

namespace AirplaneFormApplication.Services
{
    public class PassengerTicketPrintService
    {
        private Passenger? _passengerToPrint;

        public void PrintPassengerTicket(Passenger passenger)
        {
            _passengerToPrint = passenger;

            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Passenger Ticket";
            printDoc.PrintPage += PrintDoc_PrintPage;

            using (PrintDialog dlg = new PrintDialog())
            {
                dlg.Document = printDoc;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    printDoc.Print();
                }
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_passengerToPrint == null)
                return;

            var g = e.Graphics;
            float y = 50;
            float left = 60;
            var fontHeader = new Font("Arial", 16, FontStyle.Bold);
            var fontLabel = new Font("Arial", 12, FontStyle.Bold);
            var fontValue = new Font("Arial", 12, FontStyle.Regular);

            
            g.DrawString("AIRPLANE TICKET", fontHeader, Brushes.DarkBlue, left, y);
            y += 40;

            
            g.DrawString("Passenger Information", fontLabel, Brushes.Black, left, y);
            y += 30;
            g.DrawString($"Name: {_passengerToPrint.Name}", fontValue, Brushes.Black, left, y); y += 25;
            g.DrawString($"Passport Number: {_passengerToPrint.PassportNumber}", fontValue, Brushes.Black, left, y); y += 25;

            
            g.DrawString("Flight Information", fontLabel, Brushes.Black, left, y); y += 30;
            g.DrawString($"Flight ID: {_passengerToPrint.FlightId}", fontValue, Brushes.Black, left, y); y += 25;
            g.DrawString($"Seat ID: {_passengerToPrint.SeatId?.ToString() ?? "-"}", fontValue, Brushes.Black, left, y); y += 25;
            g.DrawString($"Seat Number: {_passengerToPrint.SeatNumber?.ToString() ?? "-"}", fontValue, Brushes.Black, left, y); y += 25;

            
            y += 20;
            g.DrawString("Thank you for flying with us!", fontValue, Brushes.Blue, left, y);
        }
    }
}