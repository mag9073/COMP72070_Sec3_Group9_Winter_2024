using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implementations
{
    public enum ServerState
    {
        Starting, Connected, Idle, Disconnected, Login, SignUp, AdminLogin, AllParkData, OneParkData, AllParkImages, OneParkImage, AllReviews, ParkReview, DeleteParkReview, DeleteAPark, AddAPark, AddAParkReview, EditAParkInfo
    }
    public class ServerStateManager
    {
        private static ServerState currentState = ServerState.Starting;

        public static ServerState GetCurrentState()
        {
            return currentState;
        }

        public static void SetCurrentState(ServerState newState)
        {
            Console.WriteLine($"Server state changing from {currentState} to {newState}.");
            currentState = newState;
        }
    }
}
