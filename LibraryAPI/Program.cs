namespace LibraryAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin", policy =>
				{
					policy.WithOrigins("https://localhost:7100")
							.AllowAnyHeader()
							.AllowAnyMethod();
				});
			});

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.UseCors("AllowSpecificOrigin");
			app.MapControllers();

			app.Run();
		}
	}
}
