﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatBot_Server_Milosevic
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lister: in ascolto quando si parla dei server
            // EndPoint: identifica una coppia IP/Porta

            // Creare il mio socketlistener
            //1) specifico che versione IP
            //2) tipo di socket. Stream.
            //3) protocollo a livello di trasporto
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // config: IP dove ascoltare. Possiamo usare l'opzione Any: ascolta da tutte le interfaccie all'interno del mio pc.
            IPAddress ipaddr = IPAddress.Any;

            // config: devo configurare l'EndPoint
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            // config: Bind -> collegamento
            // listenerSocket lo collego all'endpoint che ho appena configurato
            listenerSocket.Bind(ipep);

            // Mettere in ascolto il server.
            // parametro: il numero massimo di connessioni da mettere in coda.
            listenerSocket.Listen(5);
            Console.WriteLine("Server in ascolto...");
            Console.WriteLine("in attesa di connessione da parte del client...");
            // Istruzione bloccante
            // restituisce una variabile di tipo socket
            Socket client = listenerSocket.Accept();

            Console.WriteLine("Client IP: " + client.RemoteEndPoint.ToString());

            // mi attrezzo per ricevere un messaggio dal client
            // siccome è di tipo stream io riceverò dei byte, o meglio un byte array
            // riceverò anche il numero di byte.
            byte[] buff = new byte[128];
            int receivedBytes = 0;
            int sendedBytes = 0;
            string receivedString, sendString = "";

            while (true)
            {
                receivedBytes = client.Receive(buff);
                Console.WriteLine("Numero di byte ricevuti: " + receivedBytes);

                receivedString = Encoding.ASCII.GetString(buff, 0, receivedBytes);
                Console.WriteLine("Stringa ricevuta: " + receivedString);

                //if (receivedString.ToUpper() == "QUIT")
                //{
                //    break;
                //}

                if (receivedString != "\r\n")
                {
                    if (receivedString.ToUpper() == "QUIT")
                    {
                        break;
                    }

                    switch (receivedString.ToUpper())
                    {
                        case "COME STAI?":
                            sendString = "BENE";
                            break;

                        case "CHE FAI?":
                            sendString = "NIENTE";
                            break;

                        case "CIAO":
                            sendString = "CIAO";
                            break;

                        default:
                            sendString = "Non ho capito!!";
                            break;
                    }

                    Array.Clear(buff, 0, buff.Length);
                    sendedBytes = 0;

                    //lo converto in byte
                    buff = Encoding.ASCII.GetBytes(sendString);

                    //invio al client il messaggio
                    sendedBytes = client.Send(buff);

                    Array.Clear(buff, 0, buff.Length);
                }
            }

            // Termina il programma
        }
    }
}
