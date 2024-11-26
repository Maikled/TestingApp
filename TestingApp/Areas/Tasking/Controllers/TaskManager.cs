using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApp.Areas.Tasking.Models;
using TestingApp.Core.Models.Tests;
using TestingApp.Core.Processing.CSharp;
using TestingApp.Database;

namespace TestingApp.Areas.Tasking.Controllers
{
    [Area($"{nameof(Tasking)}")]
    [Route("[controller]")]
    [Authorize]
    public class TaskManager : Controller
    {
        private DatabaseContext _databaseContext { get; }

        public TaskManager(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult CreateTask(Guid userID)
        {
            ViewBag.Title = "Создание задачи";

            return View("EditTestingTask");
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult EditTask(Guid taskID)
        {
            var task = _databaseContext.Tasks.Include(p => p.Tests).Include(p => p.OwnerUser).FirstOrDefault(p => p.ID == taskID);
            ViewBag.Title = "Редактирование задачи";

            return View("EditTestingTask", task);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SaveTask([FromBody] TaskTesting task)
        {
            _databaseContext.Attach(task.OwnerUser);
            if (_databaseContext.Tasks.Any(p => p.ID == task.ID))
            {
                foreach(var test in task.Tests)
                {
                    if(_databaseContext.Tests.Any(p => p.ID == test.ID))
                    {
                        _databaseContext.Tests.Update(test);
                    }
                    else
                    {
                        await _databaseContext.Tests.AddAsync(test);
                    }
                }

                _databaseContext.Tasks.Update(task);
            }
            else
            {
                await _databaseContext.Tasks.AddAsync(task);
            }

            await _databaseContext.SaveChangesAsync();

            return Ok(task);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ShowTask(Guid taskID, Guid userID)
        {
            var task = _databaseContext.Tasks.Include(p => p.Tests).Include(p => p.OwnerUser).FirstOrDefault(p => p.ID == taskID);
            if(task != null)
            {
                var source = _databaseContext.Sources.FirstOrDefault(p => p.OwnerTaskID == taskID && p.OwnerUserID == userID);
                if(source == null)
                {
                    source = new Source()
                    {
                        ID = Guid.NewGuid(),
                        OwnerTaskID = taskID,
                        OwnerUserID = userID,
                        Code = ""
                    };

                    await _databaseContext.Sources.AddAsync(source);
                    await _databaseContext.SaveChangesAsync();
                }

                var testsExecuteHistories = new List<TestExecuteHistory>();
                foreach(var test in task.Tests)
                {
                    var testExecuteHistory = _databaseContext.TestsExecuteHistories.FirstOrDefault(p => p.TestID == test.ID && p.OwnerUserID == userID);
                    if(testExecuteHistory != null)
                    {
                        testsExecuteHistories.Add(testExecuteHistory);
                    }
                }

                var taskModel = new TaskModel()
                {
                    Task = task,
                    Source = source,
                    TestExecuteHistories = testsExecuteHistories
                };

                return View("ShowTestingTask", taskModel);
            }
            else
            {
                return BadRequest(taskID);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdateSourceCode([FromBody] Source source)
        {
            _databaseContext.Sources.Update(source);
            await _databaseContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<TestResult?> RunTest([FromBody] TestModel testModel)
        {
            var test = _databaseContext.Tests.FirstOrDefault(p => p.ID == testModel.TestID);
            if (test != null)
            {
                var testData = new TestData(testModel.Source.Code, test.InputData, test.OutputData);
                var compiler = new CSharpCompiller(testData);
                var testResult = compiler.Run();

                var testExecuteHistory = _databaseContext.TestsExecuteHistories.FirstOrDefault(p => p.TestID == testModel.TestID);
                if (testExecuteHistory == null)
                {
                    testExecuteHistory = new TestExecuteHistory()
                    { 
                        TestID = testModel.TestID,
                        IsSuccess = testResult.IsSuccess,
                        Errors = testResult.Errors,
                        OwnerUserID = testModel.Source.OwnerUserID,
                    };

                    await _databaseContext.TestsExecuteHistories.AddAsync(testExecuteHistory);
                }
                else
                {
                    testExecuteHistory.IsSuccess = testResult.IsSuccess;
                    testExecuteHistory.Errors = testResult.Errors;

                    _databaseContext.TestsExecuteHistories.Update(testExecuteHistory);
                }

                await _databaseContext.SaveChangesAsync();

                return testResult;
            }
            else
            {
                return null;
            }
        }
    }
}
