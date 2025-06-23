
using Lexiflix.Models.ViewModels;

﻿using Lexiflix.Models;
using Lexiflix.Models.Db;


namespace Lexiflix.Services
{
    public interface IOrderServices
    {
        void CreateOrder(Order order);
        //string? GetAllOrders();
        //public List<OrderViewModel> GetAllOrders();
        public PaginatedList<OrderViewModel> GetAllOrders(string searchString, int pageIndex, int pageSize);
        Movie GetMovieById(int v);

        Order GetOrderWithDetails(int id);
        void AddOrderByAdmin(OrderVM ovm);

        void UpdateOrder(OrderVM orderVM);

        void DeleteOrder(int id);

    }
}