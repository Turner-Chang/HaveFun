using HaveFun.Models;

namespace HaveFun.Common
{
	public class MembershipService
	{
		private readonly HaveFunDbContext _context;
		private readonly int dailySwipeLimit = 2;

		public MembershipService(HaveFunDbContext context)
		{
			_context = context;
		}

		public bool canUserSwipe(int userId)
		{
			if (_context.UserInfos.First(u => u.Id == userId).Level == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
