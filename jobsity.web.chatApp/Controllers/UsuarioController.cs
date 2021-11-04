using jobsity.web.chatApp.App_Code;
using jobsity.web.chatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace jobsity.web.chatApp.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel();
            model.Usuario = "";
            model.Password = "";
            model.RememberMe = true;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string ip = Request.UserHostAddress;
            string userName = model.Usuario;
            string password = model.Password;
            bool rememberMe = true;

            UserInformation userInformation = new UserInformation();
            userInformation.Usuario = userName;
            RespuestaUserInfo respuesta = Data.Login(userName, password);
            bool EsValido = respuesta.EsOK();
            if (EsValido)
            {
                userInformation = respuesta.Data;
                EsValido = userInformation.Autenticado;
            }

            if (EsValido)
            {
                FormsAuthentication.SetAuthCookie(userName, rememberMe);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            Respuesta respuesta = Data.RegisterUsuario(model);
            bool RegisterOK = respuesta.EsOK();
            if (RegisterOK)
                return RedirectToAction("Index", "Home");
            else
            {
                return View(model);
            }
        }
    }
}