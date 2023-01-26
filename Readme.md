![Roseclth](/img/rosecltth.png)

# Roseclth

Web shop application build with ASP.NET CORE MVC, Entity Framework Core, and SQLite.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine.

## Requirements
- You need to have [.NET SDK 7](https://dotnet.microsoft.com/en-us/download) installed on your machine
- Updated Visual Studio 2022 will simplify the process of launching solution, but is not necessery to use this project.

## Installing

1. Clone the repository from github or extract the project files to a directory of your choice.
2. Open the project with Visual Studio using Roseclth.sln file.
3. Restore packages by using NuGet CLI `nuget restore MySolution.sln` or by using Visual Studio GUI.
4. If you want to clear database you can delete all roseclth.db files from your project and run `Update-Database` using Package Manager Console or by using .NET CLI executing `dotnet ef database update` command.
    
## How to Run
In order to run application you need to go Roseclth.Web directory, and execute commands in terminal.
This will run application in default mode

    dotnet run

This will run application, and start watch, each time you make changes in C# code, application will restart automatically

    dotnet watch run

After running application, you should see output in your terminal.


If you are using Visual Studio simply build all projects and launch project without debugging by clicking CTRL + F5.


## Running the tests

We can run unit tests in two ways:
- You can run them in Visual Studio using GUI.
- You can run them using .NET CLI by executing `dotnet test` command in main directory.

## Users credentails

#### Admin user
    Username: admin@wsei.edu.pl
    Password: Wsei@1

#### Employee user
    Username: employee@wsei.edu.pl
    Password: Wsei@1

#### Company User
    Username: company@wsei.edu.pl
    Password: Wsei@1

#### Individual user
    Username: individual@wsei.edu.pl
    Password: Wsei@1


## Registration

Any user can access the page https://localhost:7009/Identity/Account/Register where the registration form is displayed
![Register Form](/img/register.PNG)

## Login
Any user can access the login page: https://localhost:7009/Identity/Account/Login, where an email and password must be provided.

![Login Form](/img/login.PNG)

After submitting the form, if the login data is correct, the user is redirected to the address: https://localhost:7009/ where the application's welcome page is located.

## Adding a product
An administrator account has the function of adding products to the store at the address https://localhost:7009/Admin/Product

![Product Add form](/img/additem.PNG)

## Editing a product
An administrator account has the function of editing products https://localhost:7009/Admin/Product/Upsert?id=6 (editing an example product with id=6)
![Edit product form](/img/edititem.PNG)

## Buying a product
Buying is available for logged-in users after adding products to the cart and clicking the "checkout" button, we will be transferred to the finalizing order page https://localhost:7009/Customer/Cart/Summary
![Buying product](/img/checkout.PNG)

After filling in the address details and clicking the 'Place Order' button, the user is transferred to the 'stripe' page where the order is finalized and paid

Test credit card of the STRIPE service:
Brand : Visa
Number : 4242424242424242
CVC : Any 3 digits
Date : Any future date

After paying for the order, the following message is displayed and the user is redirected to the page (for id = 13) https://localhost:7009/customer/cart/OrderConfirmation?id=13
and can check the status of their order in the 'Orders' tab.

![Final order](/img/orderstripe.PNG)
![Order status](/img/orderlist.PNG)

After clicking the "Buy Now!" button, the order is confirmed. The user is redirected to the page: https://localhost:7046/Item.
The item can be found in the My Orders tab at the address: https://localhost:7046/Order

![My orders](/img/myorders.PNG)


### Admin Panel
The admin panel is only available to users logged in to the site with administrator permissions.
To manage the admin panel, you should:

1.Log in to the administrator profile (login && password)
2.In the menu there is a drop-down tab "Admin" in which we have available functions only for the administrator
3.Select the function.

Example of using the "Materials" tab is located at the address https://localhost:7009/Admin/Material
Example view of Materials:
![Materials View](/img/materials.PNG)

Access to the admin panel is only available to users with an account on the site with administrator permissions.
To use the panel, you should use the "Admin" tab located at the address: https://localhost:7046/Admin.
To register new company / employee / admin user we need to use admin panel and create user with role that we can choose from the dropdown.

![Admin panel](/img/admin.PNG)

## Built With

* [Asp.Net Core](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
* [SQLite](https://www.sqlite.org/)

## Authors

**Kamil Żydło**

## License

This project is licensed under the MIT License - see the [LICENSE](https://en.wikipedia.org/wiki/MIT_License) for more details.