using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace EmailFormApp.Pages
{
    public class ContactModel : PageModel
    {
        public string Message { get; set; }

        [BindProperty]
        public ContactFormModel Contact { get; set; }

        public void OnGet()
        {
            Message = "Your contact page.";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page(); 
            }


            var mailbody = $@"Hallo Website Owner,

            This is new contact request from your website: 

            Name: {Contact.Name}
            Last Name: {Contact.LastName}
            Email: {Contact.Email}
            Message: ""{Contact.Message}""

            Cheers,
            The websites contact form";

            SendMail(mailbody);

            return RedirectToPage("Index");

        }

        private void SendMail(string mailbody)
        {
            using (var message = new MailMessage(Contact.Email, "noreply.cloudenergy@gmail.com"))
            {
                message.To.Add(new MailAddress("noreply.cloudenergy@gmail.com"));
                message.From = new MailAddress(Contact.Email);
                message.Subject = "New E-Mail from my website";
                message.Body = mailbody;

                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;    
                NetworkCredential creds = new NetworkCredential("noreply.cloudenergy@gmail.com", "Klemens1987", "");
                smtpClient.Credentials = creds;

                try
                {
                    smtpClient.Send(message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception is:" + ex.ToString());
                }
            
            }
        }
    }

    public class ContactFormModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }

    }
}
