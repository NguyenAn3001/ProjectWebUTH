using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class PointCashModelRequest : PointChangedModelRequest
{
    [Required(ErrorMessage = "Please enter your paypal address wallet to cash out.")]
    public string? PaypalAddress { get; set; }
}