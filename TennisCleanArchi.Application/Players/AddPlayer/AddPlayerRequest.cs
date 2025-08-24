using MediatR;
using TennisCleanArchi.Domain;

namespace TennisCleanArchi.Application.Players.AddPlayer;

public class AddPlayerRequest : IRequest<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ShortName { get; set; }
    public string Sex { get; set; }
    public string Picture { get; set; }
    public string CountryCode { get; set; }
    public PlayerStats Data { get; set; }
}
