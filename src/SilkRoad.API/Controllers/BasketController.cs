using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.API;
using SilkRoad.API.Controllers;
using SilkRoad.Core;

namespace MyApp.Namespace
{
    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
        }

        [HttpPost("add-update-basket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddUpdateBasketAsync(CustomerBasket Basket)
        {
            if(Basket is null)
                return BadRequest(new APIResponse(400));
            CustomerBasket? basket = await _uow.CustomerBasketRepository.AddUpdateBasketAsync(Basket);
            if(basket is null)
                return NotFound(new APIResponse(404));
            return Ok(basket);
        }

        [HttpDelete("delete-basket/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBasketAsync(string Id)
        {
            if(string.IsNullOrEmpty(Id))
                return BadRequest(new APIResponse(400));
            bool isDeleted = await _uow.CustomerBasketRepository.DeleteBasketAsync(Id);
            return isDeleted? Ok(new APIResponse(200))
                : NotFound(new APIResponse(404));
        }


        [HttpGet("basket/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetBasketByIdAsync(string Id)
        {
            if(string.IsNullOrEmpty(Id))
                return BadRequest(new APIResponse(400));
            CustomerBasket? basket = await _uow.CustomerBasketRepository.GetBasketByIdAsync(Id);
            if(basket is null)
                return NotFound(new APIResponse(404));
            return Ok(basket);
        }
    }
}
