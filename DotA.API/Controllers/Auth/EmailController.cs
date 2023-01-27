using System.Threading.Tasks;
using AutoMapper;
using DataAccess;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace DotA.API.Controllers.Auth
{
    [ApiController, Route("api/email")]
    public class EmailController
    {
        private readonly ApiContext _context;

        public EmailController(ApiContext context)
        {
            _context = context;
        }

        

    }
}