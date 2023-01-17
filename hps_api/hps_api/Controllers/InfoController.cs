using hps_api.DTOs;
using hps_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace hps_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : Controller
    {
        private readonly PostgresContext _context;

        public InfoController(PostgresContext context)
        {
            _context = context;
        }
        [HttpGet("get-all-infos")]
        public async Task<ActionResult<ResponseDto>> GetInfos()
        {
            var infos = await _context.Infos.OrderBy(i => i.Id).ToListAsync();

            if (infos == null || infos.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "No info list found",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "List of" + infos.Count + "infos",
                Success = true,
                Payload = new
                {
                    Output = infos
                }
            });

        }
        [HttpPost("get-info-by-id")]
        public async Task<ActionResult<ResponseDto>> GetInfos(Info myInfo)
        {
            if (myInfo.Id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "id error",
                    Success = false,
                    Payload = null
                });
            }

            var info = await _context.Infos.Where(i => i.Id >= myInfo.Id).FirstOrDefaultAsync();

            if (info == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "No Info Found",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Info Found",
                Success = true,
                Payload = info
            });
        }

        [HttpPost("create-infos"), DisableRequestSizeLimit]
        public async Task<ActionResult<ResponseDto>> PostInfo(Info info)
        {
            if (info.Id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "id is null",
                    Success = false,
                    Payload = null
                });
            }
            if (info.Name == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = " name is null",
                    Success = false,
                    Payload = null
                });
            }

            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    info.Resume = dbPath;
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                    {
                        Message = "file upload failed",
                        Success = false,
                        Payload = null
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            // file upload - end


            _context.Infos.Add(info);
            bool isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "input data is not valid",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "create done",
                Success = true,
                Payload = new { info.Id } // optional, can be null too like update
            });
        }

        [HttpPatch("update-infos")]
        public async Task<ActionResult<ResponseDto>> UpdateInfos(Info myInfo)
        {
            var info = _context.Infos.SingleOrDefault(x => x.Id == myInfo.Id);

            if (myInfo.Name == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "No Infos Found",
                    Success = false,
                    Payload = null
                });
            }else
            {
                info.Id = myInfo.Id;
                _context.Infos.Update(myInfo);
                var result = await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new ResponseDto
                {
                    Message = "Infos Updated",
                    Success = true,
                    Payload = new { myInfo.Id }
                });
            }

        }
        // DELETE: api/Countries/5 // delete (d)
        [HttpDelete("delete-infos")]
        public async Task<ActionResult<ResponseDto>> DeleteInfo(Info myInfo)
        {
            if (myInfo.Id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = " id null",
                    Success = false,
                    Payload = null
                });
            }

            Info info = await _context.Infos.Where(i => i.Id == myInfo.Id).FirstOrDefaultAsync();
            if (info == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "No info in database",
                    Success = false,
                    Payload = null
                });
            }

            _context.Infos.Remove(info);
            bool isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "didn't delete",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "deleted",
                Success = true,
                Payload = new { myInfo.Id } // optional, can be null too like update
            });
        }
    }
}
