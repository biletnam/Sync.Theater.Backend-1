﻿using Sync.Theater.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sync.Theater.EntityDataModels;

namespace Sync.Theater
{

    /// <summary>
    /// Connects to SQL database using ADO.NET
    /// </summary>
    class DatabaseConnector
    {
        private static SyncLogger Logger;


        static DatabaseConnector()
        {
            Logger = new SyncLogger("DatabaseConnector", ConsoleColor.Magenta);
        }
        /// <summary>
        /// Given the PasswordHash, Username and or Email of the user, returns a SyncUser created from database data.
        /// If an error occurs or no user is found, returns null
        /// </summary>
        /// <param name="PasswordHash"></param>
        /// <param name="Username"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static User ValidateAndGetUser(string RawPassword, string Username = "", string Email = "" )
        {
            // exit early if no username or email is provided
            if ((string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Email)) || (string.IsNullOrWhiteSpace(RawPassword))) { return null; }

            // the key we use to search for the User
            DBSearchIdentity searchIdentity;
            if (string.IsNullOrWhiteSpace(Username))
            {
                searchIdentity = DBSearchIdentity.USERNAME;
            }
            else
            {
                searchIdentity = DBSearchIdentity.EMAIL;
            }

            using (var ctx = new SyncUsersModelContainer1())
            {
                if(searchIdentity == DBSearchIdentity.USERNAME)
<<<<<<< HEAD
                {
                    return ctx.Users.Where(x => x.Username == Username && x.PasswordHash == UserAuth.HashPassword(RawPassword)).First();
                }
                else if(searchIdentity == DBSearchIdentity.EMAIL)
                {
                    return ctx.Users.Where(x => x.Email == Email && x.PasswordHash == UserAuth.HashPassword(RawPassword)).First();
                }
=======
                {
                    return ctx.Users.Where(x => x.Username == Username && x.PasswordHash == UserAuth.HashPassword(RawPassword)).First();
                }
                else if(searchIdentity == DBSearchIdentity.EMAIL)
                {
                    return ctx.Users.Where(x => x.Email == Email && x.PasswordHash == UserAuth.HashPassword(RawPassword)).First();
                }
>>>>>>> 3e57e9e410e505b26f32ad119ea55a8341570bce
                else
                {
                    return null;
                }
            }
        }

        private enum DBSearchIdentity
        {
            EMAIL,
            USERNAME
        }


        public static User AddUserToDB(string Username, string Email, string PasswordHash)
        {
            // exit early if no username or email is provided
<<<<<<< HEAD
            if ((string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email)) || (string.IsNullOrWhiteSpace(PasswordHash))) { return null; }

            var newUser = new User();

            newUser.Email = Email;
            newUser.Username = Username;
            newUser.PasswordHash = PasswordHash;

            using (var ctx = new SyncUsersModelContainer1())
            {
                var u = ctx.Users.Add(newUser);

                ctx.SaveChanges();

                return u;
            }

            
=======
            if ((string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email)) || (string.IsNullOrWhiteSpace(PasswordHash))) { return false; }

            var newUser = new User();

            newUser.Email = Email;
            newUser.Username = Username;
            newUser.PasswordHash = PasswordHash;
            int objectsWritten = 0;

            using (var ctx = new SyncUsersModelContainer1())
            {
                ctx.Users.Add(newUser);

                objectsWritten=ctx.SaveChanges();
            }

            // if successful, only one object should have been written to db
            return (objectsWritten == 1);
>>>>>>> 3e57e9e410e505b26f32ad119ea55a8341570bce
        }

        public static bool AddSyncQueueToDB(Queue queue, User user)
        {
            if (queue==null || user  ==null) { return false; }

            using (var ctx = new SyncUsersModelContainer1())
            {
                var u = ctx.Users.Where(x => x.Username == user.Username && x.PasswordHash == user.PasswordHash).First();

<<<<<<< HEAD
                if(u != null)
                {
                    u.Queues.Add(queue);
                    return true;
                }
                else
                {
                    return false;
                }
                

                
=======
                u.Queues.
>>>>>>> 3e57e9e410e505b26f32ad119ea55a8341570bce
            }
        }
    }
}
