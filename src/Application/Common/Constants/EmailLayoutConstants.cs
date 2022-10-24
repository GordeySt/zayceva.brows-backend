namespace Application.Common.Constants;

public static class EmailLayoutConstants
{
    public const string ConfirmEmailLayout = @"
            <!DOCTYPE html>
            <html lang=""en"">
              <head>
              <meta charset=""UTF-8"" />
              <title></title>
              <link rel=""preconnect"" href=""https://fonts.gstatic.com"" />
              <link
                href=""https://fonts.googleapis.com/css2?family=Rubik:wght@400;500;600;700&display=swap""
                rel=""stylesheet""
              />
              <style>
                * {
                    padding: 0;
                    margin: 0;
                 }

                p {
                    margin-bottom: 20px;
                    color: black;
                 }

                .container {
                    max-width: 700px;
                    padding: 50px;
                    font-family: ""Rubik"", sans-serif;
                    line-height: 1;
                    font-weight: 400;
                    color: #555;
                 }

                .header-title {
                   font-size: 30px;
                   font-weight: 700;
                   margin-bottom: 20px;
                 }

                .header-title span {
                   color: #edd83d;
                 }

                .btn {
                  display: inline-block;
                  text-decoration: none;
                  font-size: 20px;
                  font-weight: 600;
                  padding: 13px 20px;
                  margin-bottom: 20px;
                  border-radius: 10px;
                  background-color: #edd83d;
                  cursor: pointer;
                  font-family: inherit;
                }
    
            </style>  
         </head>
         <body>
           <div class=""container"">
             <header>
               <div class=""header-title"">zayceva<span>.</span>brows</div>
             </header>
           <main>
             <p>Hi there!</p>
             <p>
               Thanks for choosing me as your beauty master. I'm really happy to have
               you!
             </p>
              <p>Click the link below to confirm your email:</p>
              <a href=""{{verifyUrl}}"" class=""btn"" style=""color:#ffffff"">Confirm email</a>
           </main>
         </div>
       </body>
    </html>";
}