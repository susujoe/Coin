using Coin.Models;
using Coin.Models.Coin;
using Coin.ViewModels.Coin;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Coin.Controllers
{
	[Route("Error")]
	public class HomeController : Controller
	{
		[HttpGet]
		[Route("~/List")]
		public IActionResult List()
		{
			return View();
		}

		[HttpGet]
		[Route("~/Form/{formAction=Add}")]
		[Route("~/Form/{formAction=Add}/{cmCode}")]
		[Route("~/Form/{formAction=Edit}/{cmCode}/{coinMapNameId}")]
		public ActionResult Form(string formAction, string cmCode, string coinMapNameId)
		{

			ViewBag.FormAction = formAction;
			ViewBag.CoinMapNameId = coinMapNameId;

			string outputJson = string.Empty;
			BasePo baseJson = new BasePo();
			CoinViewModel formViewModel = new CoinViewModel();
			formViewModel.CMCode = cmCode;
			if (string.Equals("Add", formAction))
			{
				return View(formViewModel);
			}
			else if (string.Equals("Edit", formAction))
			{
				ViewBag.BtnDeleteEnabled = true;
				FillFormContent(ref formViewModel, coinMapNameId);
				return View(formViewModel);
			}
			else
			{
				return View(formViewModel);
			}
		}

		private void FillFormContent(ref CoinViewModel formViewModel, string CoinMapNameId)
		{
			CoinModel coinModel = new CoinModel();
			CoinPo? formPo = coinModel.GetFormData(CoinMapNameId);
			if (formPo != null)
			{
				formViewModel.CMCode = formPo.CMCode;
				formViewModel.CMNName = formPo.CMNName;
			}
		}

		[HttpGet]
		public IActionResult Error()
		{
			return View(); // 對應 Views/Home/Error.cshtml
		}
	}
}
