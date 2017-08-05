﻿using Newtonsoft.Json;
using Sync.Theater.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using Sync.Theater.Utils;
using Sync.Theater.Models;

namespace Sync.Theater
{

    public class SyncService : WebSocketBehavior
    {
        public delegate void ConnectionOpenOrCloseHandler(ConnectionAction action, SyncService s );
        public event ConnectionOpenOrCloseHandler ConnectionOpenedOrClosed = delegate { };

        public delegate void ServerMessageRecievedHandler(dynamic message, SyncService s);
        public event ServerMessageRecievedHandler ServerMessageRecieved = delegate { };

        public delegate void Client2ClientMessageRecievedHandler(dynamic message, SyncService s);
        public event Client2ClientMessageRecievedHandler Client2ClientMessageRecieved = delegate { };

        public delegate void BroadcastMessageRecievedHandler(dynamic message, SyncService s);
        public event BroadcastMessageRecievedHandler BroadcastMessageRecieved = delegate { };

        public string Nickname;

        private string UserToken;

        private UserPermissionLevel _permissions;
        public UserPermissionLevel Permissions
        {
            get
            {
                return _permissions;
            }
            set
            {
                _permissions = value;
                Send(PermissionsChanged.Notify(_permissions, this));
            }
        }

        public SyncService(SyncRoom room)
        {
            // some kind of dependency injection trick (I think..) - but it works 
            room.AddService(this);

            Nickname = GfycatNameGenerator.GetName();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            dynamic message = JsonConvert.DeserializeObject<dynamic>(e.Data);

            if(message.Recipient == MessageRecipientType.SERVER)
            {
                ServerMessageRecieved(message, this);
            }
            else if(message.Recipient == MessageRecipientType.CLIENT2CLIENT)
            {
                Client2ClientMessageRecieved(message, this);
            }
            else if (message.Recipient == MessageRecipientType.BROADCAST)
            {
                BroadcastMessageRecieved(message, this);
            }
        }

        protected override void OnOpen()
        {
            // start with the least permissions to be changed later.
            Permissions = UserPermissionLevel.VIEWER;

            ConnectionOpenedOrClosed(ConnectionAction.OPENED, this);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            
            ConnectionOpenedOrClosed(ConnectionAction.CLOSED, this);
        }

        public void SendMessage(string data)
        {
            Send(data);
        }

        
    }

    public enum MessageRecipientType
    {
        SERVER,
        CLIENT2CLIENT,
        BROADCAST
    }

    public enum ConnectionAction
    {
        OPENED,
        CLOSED
    }

    public enum UserPermissionLevel
    {
        VIEWER,
        TRUSTED,
        OWNER
    }
}
