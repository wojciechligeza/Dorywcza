namespace Dorywcza.Services.AuthService
{
    interface IAuthConfiguration
    {
        string Secret { get; set; }
    }
}
