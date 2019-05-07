using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddIdentity_ProjetoExistente.Models;
using AddIdentity_ProjetoExistente.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AddIdentity_ProjetoExistente.Controllers
{
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;

        public ManageController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'");
            }

            var model = new IndexViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'");
            }

            var email = user.Email;
            if(email != model.Email)
            {
                var setEmailRessult = await _userManager.SetEmailAsync(user, model.Email);

                if (!setEmailRessult.Succeeded)
                {
                    throw new ApplicationException($"Erro inesperado ao atribuir um e-mail para o usuário com ID '{user.Id}'");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (phoneNumber != model.PhoneNumber)
            {
                var setPhoneRessult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);

                if (!setPhoneRessult.Succeeded)
                {
                    throw new ApplicationException($"Erro inesperado ao atribuir um telefone para o usuário com ID '{user.Id}'");
                }
            }

            StatusMessage = "Seu perfil foi atualizado";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Não foi possível carregar os dados de usuário com ID {_userManager.GetUserId(User)}");
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                throw new ApplicationException($"Não foi possível carregar os dados de usuário com ID {_userManager.GetUserId(User)}");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach(var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signManager.SignInAsync(user, isPersistent: false);

            StatusMessage = "Sua senha foi alterada com sucesso";

            return RedirectToAction(nameof(ChangePassword));
        }
    }
}