using System.Net;
using System.Text.Encodings.Web;

namespace SilkRoad.Core;

public static class EmailStringBody
{
    public static string SendEmail(
        string email,
        string token,
        string component,
        string message)
    {
        string encodedEmail = WebUtility.UrlEncode(email);
        string encodedToken = WebUtility.UrlEncode(token);

        string safeMessage = HtmlEncoder.Default.Encode(message);

        return $"""
    <!DOCTYPE html>
    <html>
    <body style="
        background:#f4f6f9;
        font-family:Arial,Helvetica,sans-serif;
        padding:40px;
    ">

        <div style="
            max-width:600px;
            margin:auto;
            background:white;
            padding:40px;
            border-radius:12px;
            box-shadow:0 2px 8px rgba(0,0,0,.08);
            text-align:center;
        ">

            <h1 style="color:#222;">
                {safeMessage}
            </h1>

            <p style="
                color:#666;
                line-height:1.6;
            ">
                Click the button below to continue.
            </p>

            <a href="http://localhost:4200/{component}?email={encodedEmail}&code={encodedToken}"
               style="
                    display:inline-block;
                    background:#2563eb;
                    color:white;
                    text-decoration:none;
                    padding:14px 28px;
                    border-radius:8px;
                    font-weight:bold;
                    margin-top:20px;
               ">
                {safeMessage}
            </a>

        </div>

    </body>
    </html>
    """;
    }
}