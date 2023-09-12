﻿using AutoMapper;
using IntegratorSofttek.DTOs;
using IntegratorSofttek.Entities;
using IntegratorSofttek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IntegratorSofttek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _unitOfWork.UserRepository.GetAll();
            var usersDTO = _mapper.Map<List<UserDTO>>(users);
            return Ok(usersDTO);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);

            if (user != null)
            {
                var userDTO = _mapper.Map<UserDTO>(user);
                return Ok(userDTO);
            }
            else
            {
                return NotFound("The user couldn't be found");
            }
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            var result = await _unitOfWork.UserRepository.Insert(user);
            if (result != false)
            {
                await _unitOfWork.Complete();
                return Ok("The register operation was successful");

            }
            return BadRequest("The operation was canceled");
        }

        [HttpPut]
        [Route("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);

            var result = await _unitOfWork.UserRepository.Update(user,id);
            if (result != null)
            {
                await _unitOfWork.Complete();
                return Ok("The update operation was successful");

            }
            return BadRequest("The operation was canceled");
        }


        [HttpPut]
        [Route("DeleteSoftUser/{id}")]
        public async Task<IActionResult> DeleteSoftUser(int id)
        {

            var userReturn = await _unitOfWork.UserRepository.DeleteSoftById(id);
            if (userReturn != false)
            {
                await _unitOfWork.Complete();
                return Ok("This user has been dropped down");

            }
            return NotFound("The user couldn't be found");
        }

        [HttpDelete]
        [Route("DeleteHardUser/{id}")]
        public async Task<IActionResult> DeleteHardUser(int id)
        {
            var user = await _unitOfWork.UserRepository.DeleteHardById(id);

            if (user != null)
            {
                await _unitOfWork.Complete();
                return Ok("This user has been elimited from DataBase");
            }
            else
            {
                return NotFound("The user couldn't be found");
            }
        }
    }
}
