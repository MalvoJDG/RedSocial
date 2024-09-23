using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedSocial.Core.Application.Interfaces.Services;
using RedSocial.Core.Application.ViewModels.Friend;

namespace RedSocial.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        private readonly IFriendService _friendService;
        private readonly IPostService _postService;

        public FriendController(IFriendService friendService, IPostService postService)
        {
            _friendService = friendService;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var friends = await _friendService.GetAllViewModelWithInclude();
            var posts = await _postService.GetAllFriendsPosts(); // Obtener posts de amigos

            var homeViewModel = new HomeViewModel
            {
                Friends = friends,
                Posts = posts
            };

            return View(homeViewModel);
        }
        public async Task<IActionResult> Add()
        {
            SaveFriendViewModel vm = new();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SaveFriendViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = errors;
                return View(vm);
            }

            await _friendService.Add(vm);
            return RedirectToRoute(new { controller = "Friend", action = "Index" });
        }

        public async Task<IActionResult> Delete()
        {
            return View();
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.Delete(id);
            return RedirectToRoute(new { controller = "Friend", action = "Index" });
        }
    }
}
