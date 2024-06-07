using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;
using Microsoft.AspNetCore.Cors;
using HaveFun.DTO;

namespace HaveFun.Controllers.APIs
{

    [EnableCors("MyCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MemberProfilesApiController : ControllerBase
    {

        private readonly HaveFunDbContext _context;

        public MemberProfilesApiController(HaveFunDbContext context)
        {
            _context = context;
        }

        // GET: api/MemberProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberProfile>>> GetMemberProfile()
        {
            return await _context.MemberProfile.ToListAsync();
        }

        //POST: api/MemberProfilesApi/Filter
        [HttpPost("Filter")]
        // 篩選功能
        public async Task<IEnumerable<MemberProfile>> FilterMemberProfile(MemberProfile memberprofile)
        {
            return _context.MemberProfile.Where(
                m => m.MemberProfileId == memberprofile.MemberProfileId ||
                m.Nickname.Contains(memberprofile.Nickname) ||
                m.Interests.Contains(memberprofile.Interests)
                );
        }
        //GET: api/MemberProfilesApi/GETImage
        [HttpGet("GETImage/{id}")]
        //找到圖片
        public async Task<FileResult> GetImage(int id)
        {
            string Filename = Path.Combine("images", "noooo.jpg");
            MemberProfile m = await _context.MemberProfile.FindAsync(id);
            byte[] ImageContent = m?.Image != null ? m.Image : System.IO.File.ReadAllBytes(Filename);
            return File(ImageContent, "image/jpeg");
        }

        // GET: api/MemberProfiles/5
        [HttpGet("{id}")]
        public async Task<MemberProfile> GetMemberProfile(int id)
        {
            var memberProfile = await _context.MemberProfile.FindAsync(id);

            if (memberProfile == null)
            {
                return null;
            }
            return memberProfile;
        }

        // PUT: api/MemberProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //上傳圖片
        public async Task<string> PutMemberProfile(int id, MemberProfileDTO memberProfileDTO)
        {
            if (id != memberProfileDTO.MemberProfileId)
            {
                return "修改紀錄失敗!";
            }
            MemberProfile m = await _context.MemberProfile.FindAsync(id);
            if (m == null)
            {
                return "更新紀錄失敗~";
            }
            else
            {
                m.Nickname = memberProfileDTO.Nickname;
                m.Interests = memberProfileDTO.Interests;
                if (memberProfileDTO.Image != null)
                {
                    using (BinaryReader br = new BinaryReader(memberProfileDTO.Image.OpenReadStream()))
                    {
                        m.Image = br.ReadBytes((int)memberProfileDTO.Image.Length);
                    }
                }
            }
            _context.Entry(m).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberProfileExists(id))
                {
                    return "修改紀錄失敗!";
                }
                else
                {
                    throw;
                }
            }
            return "修改紀錄成功!";
        }

        // POST: api/MemberProfilesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<string> PostMemberProfile(MemberProfileDTO memberProfileDTO)
        {
            MemberProfile m = new MemberProfile()
            {
                Nickname = memberProfileDTO.Nickname,
                Location = memberProfileDTO.Location,
                Birthday = memberProfileDTO.BirthDay,
                Occupation = memberProfileDTO.Occupation,
                Interests = memberProfileDTO.Interests,
                Introduction = memberProfileDTO.Introduction
            };
            if (memberProfileDTO.Image != null)
            {
                using (BinaryReader br = new BinaryReader(memberProfileDTO.Image.OpenReadStream()))
                {
                    m.Image = br.ReadBytes((int)memberProfileDTO.Image.Length);
                }
            }
            _context.MemberProfile.Add(m);
            await _context.SaveChangesAsync();

            return "新增紀錄成功!";
        }

        // DELETE: api/MemberProfiles/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteMemberProfile(int id)
        {
            var memberProfile = await _context.MemberProfile.FindAsync(id);
            if (memberProfile == null)
            {
                return "刪除記錄失敗!";
            }
            try
            {
                _context.MemberProfile.Remove(memberProfile);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return "刪除關聯記錄失敗!";
            }
            return "刪除紀錄成功~";
        }

        private bool MemberProfileExists(int id)
        {
            return _context.MemberProfile.Any(e => e.MemberProfileId == id);
        }
    }

}
