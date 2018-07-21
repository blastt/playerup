using System.Web.Mvc;

namespace Market.Web.Helpers
{
    public static class EmailHelpers
    {
        public static MvcHtmlString ActivateForm(string text, string linkText, string hrefUrl)
        {
            //StringBuilder result = new StringBuilder();
            //result.Append(date.Day);
            //result.Append(" ");
            //result.Append((Monthes)date.Day);
            //result.Append(" ");
            //result.Append(date.Year);
            TagBuilder div = new TagBuilder("div");
            TagBuilder p = new TagBuilder("p");
            TagBuilder a = new TagBuilder("a");
            TagBuilder strong = new TagBuilder("strong");
            div.MergeAttribute("style", "text-align: center");
            p.SetInnerText(text);
            a.MergeAttribute("style", "background-color: #168de2; padding: 10px 30px; text-decoration:none ;font-size:14px; color:#ffffff;");
            a.MergeAttribute("href", hrefUrl);
            strong.SetInnerText(linkText);
            a.InnerHtml += strong;
            div.InnerHtml += p;
            div.InnerHtml += a;
            
            
            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString AccountData(string login, string password, string email, string emailPassword, string additionalInfo)
        {
            //StringBuilder result = new StringBuilder();
            //result.Append(date.Day);
            //result.Append(" ");
            //result.Append((Monthes)date.Day);
            //result.Append(" ");
            //result.Append(date.Year);
            TagBuilder div = new TagBuilder("div");
            TagBuilder labelLogin = new TagBuilder("label");
            TagBuilder spanLogin = new TagBuilder("span");
            spanLogin.MergeAttribute("style","width: 100%");
            TagBuilder labelPassword = new TagBuilder("label");
            TagBuilder spanPassword = new TagBuilder("span");
            spanPassword.MergeAttribute("style", "width: 100%");
            TagBuilder labelEmail = new TagBuilder("label");
            TagBuilder spanEmail = new TagBuilder("span");
            spanEmail.MergeAttribute("style", "width: 100%");
            TagBuilder labelEmailPass = new TagBuilder("label");
            TagBuilder spanEmailPass = new TagBuilder("span");
            spanEmailPass.MergeAttribute("style", "width: 100%");
            TagBuilder labelAdd = new TagBuilder("label");
            TagBuilder spanAdd = new TagBuilder("span");
            spanAdd.MergeAttribute("style", "width: 100%");
            TagBuilder div1 = new TagBuilder("div");
            TagBuilder div2 = new TagBuilder("div");
            TagBuilder div3 = new TagBuilder("div");
            TagBuilder div4 = new TagBuilder("div");
            TagBuilder div5 = new TagBuilder("div");

            div1.MergeAttribute("style", "width: 100%");
            div2.MergeAttribute("style", "width: 100%");
            div3.MergeAttribute("style", "width: 100%");
            div4.MergeAttribute("style", "width: 100%");
            div5.MergeAttribute("style", "width: 100%");

            
            labelLogin.SetInnerText("Логин аккаунта:");
            spanLogin.SetInnerText(login);
            div1.InnerHtml += labelLogin;
            div1.InnerHtml += spanLogin;
            labelPassword.SetInnerText("пароль аккаунта:");
            spanPassword.SetInnerText(password);
            div2.InnerHtml += labelPassword;
            div2.InnerHtml += spanPassword;
            labelEmail.SetInnerText("Почта:");
            spanEmail.SetInnerText(email);
            div3.InnerHtml += labelEmail;
            div3.InnerHtml += spanEmail;
            labelEmailPass.SetInnerText("Пароль почты:");
            spanEmailPass.SetInnerText(emailPassword);
            div4.InnerHtml += labelEmailPass;
            div4.InnerHtml += spanEmailPass;
            labelAdd.SetInnerText("Дополнительная информация:");
            spanAdd.SetInnerText(additionalInfo);
            div5.InnerHtml += labelAdd;
            div5.InnerHtml += spanAdd;

            div.InnerHtml += div1;
            div.InnerHtml += div2;
            div.InnerHtml += div3;
            div.InnerHtml += div4;
            div.InnerHtml += div5;



            return new MvcHtmlString(div.ToString());
        }
    }
}