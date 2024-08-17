using Final_Core.Enums;

namespace Final_Business.DTOs.General;

public record CommentGetDto(
  int Id,
  string Content,
  string AppUserId,
  int HouseId,
  DateTime CreatedAt,
  string AppUserAvatarLink,
  string AppUserUserName,
  CommentStatus Status
);

public record CommentCreateDto(
  string Content,
  string AppUserId,
  int HouseId
);