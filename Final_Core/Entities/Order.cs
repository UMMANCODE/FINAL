﻿using Final_Core.Enums;

namespace Final_Core.Entities;
public class Order {
  public int Id { get; set; }
  public string AppUserId { get; set; }
  public AppUser AppUser { get; set; }
  public int HouseId { get; set; }
  public House House { get; set; }
  public decimal Price { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public string Address { get; set; }
  public OrderStatus Status { get; set; } = OrderStatus.Pending;
}
