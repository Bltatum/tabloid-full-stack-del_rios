﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tabloid.Data;
using Tabloid.Models;

namespace Tabloid.Repositories
{
    public class UserProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public UserProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            return _context.UserProfile
                       .Include(up => up.UserType) 
                       .FirstOrDefault(up => up.FirebaseUserId == firebaseUserId);
        }

        public UserProfile GetByUserId(int id)
        {
            return _context.UserProfile
                       .Include(up => up.UserType)
                       .FirstOrDefault(up => up.Id == id);
        }

        public List<UserProfile> GetAll()
        {
            return _context.UserProfile
                .Include(up => up.UserType)
                .OrderBy(up => up.DisplayName)
                .ToList();
                
        }

        public void Add(UserProfile userProfile)
        {
            _context.Add(userProfile);
            _context.SaveChanges();
        }

        public NotImplementedException Update(UserProfile userProfile)
        {
            return new NotImplementedException();
        }
    }
}
