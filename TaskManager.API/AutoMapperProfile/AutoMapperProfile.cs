using AutoMapper;
using TaskManager.API.DTOs;
using TaskManager.API.Models;

namespace TaskManager.API.AutoMapperProfile;

public class AutoMapperProfile:Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TaskItem, TaskItemReponseDto>();
        CreateMap<CreateTaskItemDto, TaskItem>();
        CreateMap<UpdateTaskItemDto, TaskItem>();

        CreateMap<Category, CategoryResponseDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpadateCategoryDto, Category>();

        CreateMap<Comment, CommentResponseDto>();
        CreateMap<CreateCommentDto, Comment>();
    }
}
