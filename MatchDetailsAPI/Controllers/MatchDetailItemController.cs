using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MatchDetailsAPI.Interfaces;
using MatchDetailsAPI.Models;

namespace MatchDetailsAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchDetailItemsController : Controller
    {
        private readonly IMatchDetailRepository _matchDetailRepository;

        public MatchDetailItemsController(IMatchDetailRepository matchDetailRepository)
        {
            _matchDetailRepository = matchDetailRepository;
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(_matchDetailRepository.All);
        }

        [HttpPost]
        public IActionResult Create([FromBody]MatchDetailItem item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.MatchDetailItemNameAndNotesRequired.ToString());
                }
                bool itemExists = _matchDetailRepository.DoesItemExist(item.ID);
                if (itemExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.MatchDetailItemIDInUse.ToString());
                }
                _matchDetailRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            return Ok(item);
        }

        [HttpPut]
        public IActionResult Edit([FromBody] MatchDetailItem item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.MatchDetailItemNameAndNotesRequired.ToString());
                }
                var existingItem = _matchDetailRepository.Find(item.ID);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _matchDetailRepository.Update(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var item = _matchDetailRepository.Find(id);
                if (item == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _matchDetailRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            return NoContent();
        }
    }

    public enum ErrorCode
    {
        MatchDetailItemNameAndNotesRequired,
        MatchDetailItemIDInUse,
        RecordNotFound,
        CouldNotCreateItem,
        CouldNotUpdateItem,
        CouldNotDeleteItem
    }

}