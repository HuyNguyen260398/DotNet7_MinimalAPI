using AutoMapper;
using FluentValidation;
using MagicVilla_Coupon_API;
using MagicVilla_Coupon_API.Data;
using MagicVilla_Coupon_API.Models;
using MagicVilla_Coupon_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("api/coupon", (ILogger<Program> logger) => {
    logger.Log(LogLevel.Information, "Getting all Coupons");

    APIResponse response = new();
    response.Result = CouponStore.couponList;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);
})
.WithName("GetCoupons")
.Produces<APIResponse>(200);

app.MapGet("api/coupon/{id:int}", (int id) => {
    APIResponse response = new();
    response.Result = CouponStore.couponList.FirstOrDefault(x => x.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);
})
.WithName("GetCouponById")
.Produces<APIResponse>(200);

app.MapPost("api/coupon", async ([FromBody] CouponCreateDTO coupon_C_DTO, IMapper mapper, IValidator<CouponCreateDTO> validator) => {
    APIResponse response = new()
    {
        IsSuccess = false,
        StatusCode = HttpStatusCode.BadRequest
    };

    //var validationResult = validator.Validate(coupon_C_DTO);
    var validationResult = await validator.ValidateAsync(coupon_C_DTO);
    
    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }

    if (CouponStore.couponList.FirstOrDefault(x => x.Name.ToLower() == coupon_C_DTO.Name.ToLower()) is not null)
    {
        response.ErrorMessages.Add("Coupon Name is already Exists");
        return Results.BadRequest(response);
    }

    Coupon coupon = mapper.Map<Coupon>(coupon_C_DTO);
    coupon.Id = CouponStore.couponList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupon);

    CouponDTO couponDTO = mapper.Map<CouponDTO>(coupon);

    response.Result = couponDTO;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;

    //return Results.Created($"/api/coupon/{coupon.Id}", coupon);
    //return Results.CreatedAtRoute("GetCouponById", new {id=coupon.Id}, couponDTO);
    return Results.Ok(response);
})
.WithName("CreateCoupon")
.Accepts<CouponCreateDTO>("application/json")
.Produces<APIResponse>(201)
.Produces(400);

app.MapPut("api/coupon", () => {

});

app.MapDelete("api/coupon/{id:int}", (int id) => {

});

app.UseHttpsRedirection();

app.Run();
