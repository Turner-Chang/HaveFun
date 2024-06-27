

	public class BackendAuthMiddleware
	{
		private readonly RequestDelegate _next;

		public BackendAuthMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// 假設 session string "Login" 用於判斷是否已登入
			if (context.Request.Path.StartsWithSegments("/ManagementSystem") &&
				!context.Request.Path.StartsWithSegments("/ManagementSystem/Login") &&
				string.IsNullOrEmpty(context.Session.GetString("Login")))
			{
				// 未登入，重定向到後台登入頁面
				context.Response.Redirect("/ManagementSystem/Login");
				return;
			}

			// 已登入，繼續處理請求
			await _next(context);
		}
	}
