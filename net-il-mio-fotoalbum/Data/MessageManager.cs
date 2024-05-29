using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
    public class MessageManager
    {
        public static void SaveMessage(Message message)
        {
            using (var db = new PhotoDbContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }
    }
}
