using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
    public class Task
    {
        private int _id;
        private string _description;
        private string _dueDate;
        private bool _completed;

        public Task(string Description, string DueDate, bool Completed = false, int Id = 0)
        {
            _id = Id;
            _description = Description;
            _dueDate = DueDate;
            _completed = Completed;
        }

        public int GetId()
        {
            return _id;
        }
        public string GetDescription()
        {
            return _description;
        }
        public void SetDescription(string newDescription)
        {
            _description = newDescription;
        }

        public string GetDueDate()
        {
            return _dueDate;
        }
        public void SetDueDate(string newDueDate)
        {
            _dueDate = newDueDate;
        }

        public bool GetCompleted()
        {
            return _completed;
        }

        public void SetCompleted(bool newCompleted)
        {
            _completed = newCompleted;
        }

        public bool CompletedIntToBool(int number)
        {
            if (number == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int TranslateComplete()
        {
            if (this._completed == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override bool Equals(System.Object otherTask)
        {
            if (!(otherTask is Task))
            {
                return false;
            }
            else
            {
                Task newTask = (Task) otherTask;
                bool idEquality = this.GetId() == newTask.GetId();
                bool descriptionEquality = this.GetDescription() == newTask.GetDescription();
                bool dueDateEquality = this.GetDueDate() == newTask.GetDueDate();
                bool completedEquality = this.GetCompleted() == newTask.GetCompleted();
                return (idEquality && descriptionEquality && dueDateEquality && completedEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetDescription().GetHashCode();
        }

        public static List<Task> GetAll()
        {
            List<Task> AllTasks = new List<Task>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM tasks", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int taskId = rdr.GetInt32(0);
                string taskDescription = rdr.GetString(1);
                string taskDueDate = rdr.GetString(2);
                bool taskCompleted;
                if (rdr.GetByte(3) == 1)
                {
                    taskCompleted = true;
                }
                else{
                    taskCompleted = false;
                }
                Task newTask = new Task(taskDescription, taskDueDate, taskCompleted, taskId);
                AllTasks.Add(newTask);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return AllTasks;
        }

        public static List<Task> OrderByDate()
        {
            List<Task> AllTasks = new List<Task>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM tasks ORDER BY duedate;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int taskId = rdr.GetInt32(0);
                string taskDescription = rdr.GetString(1);
                string taskDueDate = rdr.GetString(2);
                bool taskCompleted;
                if (rdr.GetByte(3) == 1)
                {
                    taskCompleted = true;
                }
                else{
                    taskCompleted = false;
                }
                Task newTask = new Task(taskDescription, taskDueDate, taskCompleted, taskId);
                AllTasks.Add(newTask);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return AllTasks;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description, duedate, completed) OUTPUT INSERTED.id VALUES (@TaskDescription, @TaskDueDate, @TaskCompleted);", conn);

            SqlParameter descriptionParameter = new SqlParameter();
            descriptionParameter.ParameterName = "@TaskDescription";
            descriptionParameter.Value = this.GetDescription();

            SqlParameter dueDateParameter = new SqlParameter();
            dueDateParameter.ParameterName = "@TaskDueDate";
            dueDateParameter.Value = this.GetDueDate();

            SqlParameter completedParameter = new SqlParameter();
            completedParameter.ParameterName = "@TaskCompleted";
            completedParameter.Value = this.TranslateComplete();

            cmd.Parameters.Add(descriptionParameter);
            cmd.Parameters.Add(dueDateParameter);
            cmd.Parameters.Add(completedParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }

        public void AddCategory(Category newCategory)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO categories_tasks (category_id, task_id) VALUES (@CategoryId, @TaskId);", conn);

            SqlParameter categoryIdParameter = new SqlParameter();
            categoryIdParameter.ParameterName = "@CategoryId";
            categoryIdParameter.Value = newCategory.GetId();
            cmd.Parameters.Add(categoryIdParameter);

            SqlParameter taskIdParameter = new SqlParameter();
            taskIdParameter.ParameterName = "@TaskId";
            taskIdParameter.Value = this.GetId();
            cmd.Parameters.Add(taskIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Category> GetCategories()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT category_id FROM categories_tasks WHERE task_id = @TaskId;", conn);

            SqlParameter taskIdParameter = new SqlParameter();
            taskIdParameter.ParameterName = "@TaskId";
            taskIdParameter.Value = this.GetId();
            cmd.Parameters.Add(taskIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            List<int> categoryIds = new List<int> {};

            while (rdr.Read())
            {
                int categoryId = rdr.GetInt32(0);
                categoryIds.Add(categoryId);
            }
            if (rdr != null)
            {
                rdr.Close();
            }

            List<Category> categories = new List<Category> {};

            foreach (int categoryId in categoryIds)
            {
                SqlCommand categoryQuery = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);

                SqlParameter categoryIdParameter = new SqlParameter();
                categoryIdParameter.ParameterName = "@CategoryId";
                categoryIdParameter.Value = categoryId;
                categoryQuery.Parameters.Add(categoryIdParameter);

                SqlDataReader queryReader = categoryQuery.ExecuteReader();
                while (queryReader.Read())
                {
                    int thisCategoryId = queryReader.GetInt32(0);
                    string categoryName = queryReader.GetString(1);
                    Category foundCategory = new Category(categoryName, thisCategoryId);
                    categories.Add(foundCategory);
                }
                if (queryReader != null)
                {
                    queryReader.Close();
                }
            }
            if (conn != null)
            {
                conn.Close();
            }
            return categories;
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static Task Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
            SqlParameter taskIdParameter = new SqlParameter();
            taskIdParameter.ParameterName = "@TaskId";
            taskIdParameter.Value = id.ToString();
            cmd.Parameters.Add(taskIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundTaskId = 0;
            string foundTaskDescription = null;
            string foundTaskDueDate = null;
            bool foundTaskCompleted = false;

            while(rdr.Read())
            {
                foundTaskId = rdr.GetInt32(0);
                foundTaskDescription = rdr.GetString(1);
                foundTaskDueDate = rdr.GetString(2);
                if (rdr.GetByte(3) == 1)
                {
                    foundTaskCompleted = true;
                }
                else{
                    foundTaskCompleted = false;
                }
            }
            Task foundTask = new Task(foundTaskDescription, foundTaskDueDate, foundTaskCompleted, foundTaskId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundTask;
        }

        public static List<Task> CompletedTasks()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            List<Task> completedList = new List<Task>{};

            SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE completed = 1;", conn);
            // SqlParameter taskIdParameter = new SqlParameter();
            // taskIdParameter.ParameterName = "@TaskCompleted";
            // taskIdParameter.Value = 1.ToString();
            // cmd.Parameters.Add(taskIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();


            while(rdr.Read())
            {
                int foundTaskId = rdr.GetInt32(0);
                string foundTaskDescription = rdr.GetString(1);
                string foundTaskDueDate = rdr.GetString(2);
                bool foundTaskCompleted;
                if (rdr.GetByte(3) == 1)
                {
                    foundTaskCompleted = true;
                }
                else{
                    foundTaskCompleted = false;
                }
                Task newTask = new Task(foundTaskDescription, foundTaskDueDate, foundTaskCompleted, foundTaskId);
                completedList.Add(newTask);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return completedList;
        }

        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM tasks WHERE id = @TaskId; DELETE FROM categories_tasks WHERE task_id = @TaskId;", conn);
            SqlParameter taskIdParameter = new SqlParameter();
            taskIdParameter.ParameterName = "@TaskId";
            taskIdParameter.Value = this.GetId();

            cmd.Parameters.Add(taskIdParameter);
            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public void MarkComplete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE tasks SET completed = 1 OUTPUT INSERTED.completed WHERE id = @taskId;", conn);

            SqlParameter taskIdParameter = new SqlParameter();
            taskIdParameter.ParameterName = "@taskId";
            taskIdParameter.Value = this.GetId();
            cmd.Parameters.Add(taskIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                if (rdr.GetByte(0) == 1)
                {
                    this._completed = true;
                }
                else{
                    this._completed = false;
                }
            }

            if (rdr != null)
            {
                rdr.Close();
            }

            if (conn != null)
            {
                conn.Close();
            }
        }

    }
}
