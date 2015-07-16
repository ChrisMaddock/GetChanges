﻿using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alteridem.GetChanges
{
    /// <summary>
    /// A simple class for communicating with GitHub
    /// </summary>
    internal class GitHubApi
    {
        GitHubClient _github = new GitHubClient(new ProductHeaderValue("Alteridem.GetChangeset"));
        string _organization;
        string _repository;

        /// <summary>
        /// Constructs a class for talking to GitHub
        /// </summary>
        /// <param name="organization">The organization you are interested in</param>
        /// <param name="repository">The repository within an organization</param>
        public GitHubApi(string organization, string repository)
        {
            _organization = organization;
            _repository = repository;
        }

        public async Task<IReadOnlyList<Milestone>> GetAllMilestones()
        {
            var request = new MilestoneRequest();
            request.State = ItemState.All;
            request.SortProperty = MilestoneSort.DueDate;
            request.SortDirection = SortDirection.Descending;
            try
            {
                return await _github.Issue.Milestone.GetAllForRepository(_organization, _repository, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get milestones for repository, {0}", ex.Message);
            }
            return new List<Milestone>();
        }

        public async Task<IReadOnlyList<Issue>> GetClosedIssues()
        {
            var request = new RepositoryIssueRequest();
            request.State = ItemState.Closed;
            request.SortProperty = IssueSort.Created;
            request.SortDirection = SortDirection.Ascending;
            try
            {
                return await _github.Issue.GetAllForRepository(_organization, _repository, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get issues for repository, {0}", ex.Message);
            }
            return new List<Issue>();
        }
    }
}