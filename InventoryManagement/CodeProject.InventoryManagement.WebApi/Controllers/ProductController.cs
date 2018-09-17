﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using CodeProject.InventoryManagement.Interfaces;
using CodeProject.InventoryManagement.Data.Transformations;
using CodeProject.InventoryManagement.WebApi.ActionFilters;
using CodeProject.Shared.Common.Models;
using CodeProject.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using CodeProject.Shared.Common.Interfaces;
using Microsoft.Extensions.Options;
using CodeProject.InventoryManagement.WebApi.SignalRHub;
using Microsoft.AspNetCore.SignalR;

namespace CodeProject.InventoryManagement.WebApi.Controllers
{
	[ServiceFilter(typeof(SecurityFilter))]
	[Authorize]
	[Route("api/[controller]")]
	[EnableCors("SiteCorsPolicy")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IInventoryManagementBusinessService _inventoryManagementBusinessService;

		private IHubContext<MessageQueueHub> _messageQueueContext;

		public IConfiguration configuration { get; }

		/// <summary>
		/// Product Controller
		/// </summary>
		public ProductController(IInventoryManagementBusinessService inventoryManagementBusinessService, IHubContext<MessageQueueHub> messageQueueContext)
		{
			_inventoryManagementBusinessService = inventoryManagementBusinessService;
			_messageQueueContext = messageQueueContext;
		}

		/// <summary>
		/// Register User
		/// </summary>
		/// <param name="productDataTransformation"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("CreateProduct")]
		public async Task<IActionResult> CreateProduct([FromBody] ProductDataTransformation productDataTransformation)
		{

			SecurityModel securityModel = (SecurityModel)(HttpContext.Items["SecurityModel"]);

			int accountId = securityModel.AccountId;

			productDataTransformation.AccountId = accountId;

			ResponseModel<ProductDataTransformation> returnResponse = new ResponseModel<ProductDataTransformation>();
			try
			{
				returnResponse = await _inventoryManagementBusinessService.CreateProduct(productDataTransformation);
				returnResponse.Token = securityModel.Token;
				if (returnResponse.ReturnStatus == false)
				{
					return BadRequest(returnResponse);
				}

				await _messageQueueContext.Clients.All.SendAsync("SendMessage", string.Empty);

				return Ok(returnResponse);
				
			}
			catch (Exception ex)
			{
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
				return BadRequest(returnResponse);
			}

		}

		/// <summary>
		/// Update Product
		/// </summary>
		/// <param name="productDataTransformation"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("UpdateProduct")]
		public async Task<IActionResult> UpdateProduct([FromBody] ProductDataTransformation productDataTransformation)
		{

			SecurityModel securityModel = (SecurityModel)(HttpContext.Items["SecurityModel"]);

			int accountId = securityModel.AccountId;

			productDataTransformation.AccountId = accountId;

			ResponseModel<ProductDataTransformation> returnResponse = new ResponseModel<ProductDataTransformation>();
			try
			{
				returnResponse = await _inventoryManagementBusinessService.UpdateProduct(productDataTransformation);
				returnResponse.Token = securityModel.Token;
				if (returnResponse.ReturnStatus == false)
				{
					return BadRequest(returnResponse);
				}

				await _messageQueueContext.Clients.All.SendAsync("SendMessage", string.Empty);

				return Ok(returnResponse);
				
			}
			catch (Exception ex)
			{
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
				return BadRequest(returnResponse);
			}

		}


	}
}