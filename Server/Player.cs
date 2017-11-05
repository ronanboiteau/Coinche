using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Player
    {
        private int id;
        private String name;
        private TcpClient channel;

        public Player(int id, String name, TcpClient channel)
        {
            this.id = id;
            this.name = name;
            this.channel = channel;
        }

        public void SendMessage(String message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message + "\n");
            channel.GetStream().Write(data, 0, data.Length);
        }

        public String GetName()
        {
            return name;
        }

        public int GetId()
        {
            return id;
        }
    }
}