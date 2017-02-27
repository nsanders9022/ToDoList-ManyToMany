using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Category.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Category firstCategory = new Category("Household chores");
      Category secondCategory = new Category("Household chores");

      //Assert
      Assert.Equal(firstCategory, secondCategory);
    }

    [Fact]
    public void Test_Save_SavesCategoryToDatabase()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      //Act
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCategoryObject()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      //Act
      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCategoryInDatabase()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      //Act
      Category foundCategory = Category.Find(testCategory.GetId());

      //Assert
      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Test_GetTasks_ReturnsAllCategoryTasks()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task testTask1 = new Task("Mow the lawn", "01-02-2017");
      testTask1.Save();

      Task testTask2 = new Task("Buy plane ticket", "01-02-2017");
      testTask2.Save();

      //Act
      testCategory.AddTask(testTask1);
      List<Task> savedTasks = testCategory.GetTasks();
      List<Task> testList = new List<Task> {testTask1};

      //Assert
      Assert.Equal(testList, savedTasks);
    }

   [Fact]
   public void Test_Delete_DeletesCategoryFromDatabase()
   {
     //Arrange
     string name1 = "Home stuff";
     Category testCategory1 = new Category(name1);
     testCategory1.Save();

     string name2 = "Work stuff";
     Category testCategory2 = new Category(name2);
     testCategory2.Save();

     //Act
     testCategory1.Delete();
     List<Category> resultCategories = Category.GetAll();
     List<Category> testCategoryList = new List<Category> {testCategory2};

     //Assert
     Assert.Equal(testCategoryList, resultCategories);
   }

   [Fact]
   public void Test_AddTaskToCategory()
   {
     //Arrange
     Category testCategory = new Category("Household chores");
     testCategory.Save();

     Task testTask = new Task("Mow the lawn", "01-02-2017");
     testTask.Save();

     Task testTask2 = new Task("Water the garden", "01-02-2017");
     testTask2.Save();

     //Act
     testCategory.AddTask(testTask);
     testCategory.AddTask(testTask2);

     List<Task> result = testCategory.GetTasks();
     List<Task> testList = new List<Task>{testTask, testTask2};

     //Assert
     Assert.Equal(testList, result);
   }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }
  }
}
