using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace test.HubS
{
    public class HabrDotNetHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly BlogContext _context;

        public HabrDotNetHub(UserManager<User> userManager,BlogContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SendMessage(string ArticleID, string Text)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            Comment comment = new Comment();
            comment.User = user;
            comment.DateTime = DateTime.Now;
            comment.PostId = Int32.Parse(ArticleID);
            comment.Text = Text;
            _context.Add(comment);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", user.UserName, comment.Text, comment.DateTime);
        }
    }
}
