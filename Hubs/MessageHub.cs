using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NetworkScannerBackend.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace NetworkScannerBackend.Hubs
{
    public class MessageHub : Hub
    {
        Result result = new Result();
        Models.EndPoint endPoint = new Models.EndPoint();
        Numbers numbers = new Numbers();
        List<int> ListFirstIPs = new List<int>();
        List<int> ListSecondIPs = new List<int>();
        List<int> ListThirdIPs = new List<int>();
        List<int> ListFourthIPs = new List<int>();
        Ping pingSender = new Ping();

        public async Task<Result> Send(string data)
        {
            string[] strs = data.Split(',');
            var StartIP = strs[0];
            var EndIP = strs[1];
            OutNum();
            async void OutNum(){
                
                string[] SplitStartIP = StartIP.Split('.');
                this.numbers.FirstStartIP = Int32.Parse(SplitStartIP[0]);
                this.numbers.SecondStartIP = Int32.Parse(SplitStartIP[1]);
                this.numbers.ThirdStartIP = Int32.Parse(SplitStartIP[2]);
                this.numbers.FourthStartIP = Int32.Parse(SplitStartIP[3]);

                string[] SplitEndIP = EndIP.Split('.');
                this.numbers.FirstEndIP = Int32.Parse(SplitEndIP[0]);
                this.numbers.SecondEndIP = Int32.Parse(SplitEndIP[1]);
                this.numbers.ThirdEndIP = Int32.Parse(SplitEndIP[2]);
                this.numbers.FourthEndIP = Int32.Parse(SplitEndIP[3]);

                if (this.numbers.FirstStartIP < this.numbers.FirstEndIP)
                {
                    for (int i = this.numbers.FirstStartIP; i <= this.numbers.FirstEndIP; i++)
                    {
                        ListFirstIPs.Add(i);
                    }
                }
                else
                {
                    ListFirstIPs.Add(this.numbers.FirstStartIP);
                }

                if (this.numbers.SecondStartIP < this.numbers.SecondEndIP)
                {
                    for (int i = this.numbers.SecondStartIP; i <= this.numbers.SecondEndIP; i++)
                    {
                        ListSecondIPs.Add(i);
                    }
                }
                else
                {
                    ListSecondIPs.Add(this.numbers.SecondStartIP);
                }
                  
                if (this.numbers.ThirdStartIP < this.numbers.ThirdEndIP)
                {
                    for (int i = this.numbers.ThirdStartIP; i <= this.numbers.ThirdEndIP; i++)
                    {
                        ListThirdIPs.Add(i);
                    }
                }
                else
                {
                    ListThirdIPs.Add(this.numbers.ThirdStartIP);
                }
                   
                if (this.numbers.FourthStartIP < this.numbers.FourthEndIP)
                {
                    for (int i = this.numbers.FourthStartIP; i <= this.numbers.FourthEndIP; i++)
                    {
                        ListFourthIPs.Add(i);
                    }
                }
                else
                {
                    ListFourthIPs.Add(this.numbers.FourthStartIP);
                }
                //foreach//
                foreach (var FirstIP in ListFirstIPs)
                {
                    this.numbers.Address = $"{FirstIP}.{ListSecondIPs[0]}.{ListThirdIPs[0]}.{ListFourthIPs[0]}";
                    SendPing();
                }
                foreach (var SecondIP in ListSecondIPs)
                {
                    this.numbers.Address = $"{ListFirstIPs[0]}.{SecondIP}.{ListThirdIPs[0]}.{ListFourthIPs[0]}";
                    SendPing();
                }

                foreach (var ThirdIP in ListThirdIPs)
                {
                    this.numbers.Address = $"{ListFirstIPs[0]}.{ListSecondIPs[0]}.{ThirdIP}.{ListFourthIPs[0]}";
                    SendPing();
                }
                foreach (var FourthIP in ListFourthIPs)
                {
                    this.numbers.Address = $"{ListFirstIPs[0]}.{ListSecondIPs[0]}.{ListThirdIPs[0]}.{FourthIP}";
                    SendPing();
                }
            }
            async void SendPing() {
                Ping pingSender = new Ping();
                int timeout = 10000;
                string d = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(d);
                PingOptions options = new PingOptions(64, true);
                PingReply reply = pingSender.Send(this.numbers.Address, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    endPoint.ip = reply.Address.ToString();
                    endPoint.isUP = true;
                    endPoint.ms = reply.RoundtripTime.ToString();
                    result.status = 200;
                    result.message = "OK";
                    result.endPoint = endPoint;
                    await Clients.All.SendAsync("Send", result);
                }
                else
                {
                    endPoint.ip = reply.Address.ToString();
                    result.message = reply.Status.ToString();
                    endPoint.isUP = false;
                    result.status = 400;
                    result.endPoint = endPoint;
                    await Clients.All.SendAsync("Send", result);
                }
            }
            return result;
        }
    }
}
