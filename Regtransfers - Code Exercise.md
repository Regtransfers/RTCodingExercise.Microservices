 ![][image1]

# 

# 

# Regtransfers

# Coding Exercise

Last updated: 12th January 2022  
Overview

### Objectives

Demonstrate clean, readable, and maintainable code with evidence of a test first approach, decent code coverage and application of SOLID design principles

### Time

Spend as long as you would like on this test (we recommend 2+ hours), the more you complete the more we are able to gauge your ability. You are not required to complete all the user stories, you can even pick and choose which ones to do (although there are some that have prerequisites).

### Requirements

- .NET 6 SDK: [https://dotnet.microsoft.com/en-us/download/dotnet/6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)  
- IDE (Visual Studio, or Visual Studio Code)  
  - Visual Studio Community (Free): [https://visualstudio.microsoft.com/vs/community/](https://visualstudio.microsoft.com/vs/community/)  
  - Visual Studio Code (Free): [https://code.visualstudio.com/](https://code.visualstudio.com/)  
- Docker Desktop (Required for Microservices version): [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)

### Provisions

- [https://github.com/Regtransfers/RTCodingExercise-Microservices](https://github.com/Regtransfers/RTCodingExercise-Microservices)  
- You have been provided with a solution with four projects  
  - ASP.NET 6 MVC Project  
  - ASP.NET 6 API Project for a Catalogue of Plates  
    - DbContext  
    - Model for Plates  
    - Seed Method to provide sample data  
  - xUnit Test Project for each of these (you may change this out to nUnit if you prefer)  
  - docker-compose file  
  - This project has a MassTransit and RabbitMQ message bus for your utilisation  
    - [https://masstransit-project.com/](https://masstransit-project.com/)  
    - MassTransit has been bootstrapped and useful comments added in the Program.cs files.

## Requirements

To build a new Registration Number Plate search system\!

### User Story 1

As the head of commercial operations  
I would like to be able to build a list of available plates for sale  
So that we can market them to the public 

#### Acceptance Criteria

Given I have a list of plates  
Then I would like to be able to output the list showing the Plate, the PurchasePrice and the SalePrice  
Given I have a list of plates  
When I add a new plate  
Then I would like to be able to output the list including the new plate

Given I have a list of plates  
When viewing the list, the SalePrice should include 20% markup.

\[Advanced\]  
Given i have 60,000,000 plates (we have only provided a small number of plates for the purpose of this exercise)  
When viewing the list  
Then i should only see 20 plates on the page, and be able to page through remainder

## 

## User Story 2

As the head of marketing  
I would like to be able to order the list of plates by price   
So that our customers can view plates in their price range

#### Acceptance Criteria

Given I have a list of plates  
I would like to be able to order the list by price  
Then have the plates displayed in price order

## 

## User Story 3

As the head of marketing  
I would like to be able to filter the list of plates by Numbers or Letters   
So that our customers can find plates that match their initials / age

#### Acceptance Criteria

Given I have a list of plates  
I would like to be able to type in my age or initials  
Then have the plates displayed that contain my filter criteria

\[Advanced\]  
Given I have a list of plates  
When entering my name into the filter  
Then i expect a list of plates that look like my name  
i.e.

| Danny | ![][image2] | DA12 NNY |
| :---- | :---: | :---- |
| G Smith | ![][image3]  | GSM 17H |
| James | ![][image4]  | JAM 3S |

## 

## User Story 4

As the head of commercial operations  
I would like to mark plates as reserved  
So that we are not selling plates that are reserved

Acceptance Criteria

Given I have a list of plates  
When I configure a plate to be reserved  
And I search for the plate   
Then the plate should have a status of reserved

\[Advanced\]  
Given a plate is reserved/unreserved  
Then that information should be logged for audit purposes

## 

## User Story 5

As the head of commercial operations  
I would like only plates “for sale” to appear in a filtered search  
So that we do not confuse customers

Acceptance Criteria

Given I have a list of plates with some that are reserved  
When I output the list from search  
Then only plates for sale should be shown

## 

## User Story 6

As the sales director  
I would like to be able to sell a plate  
So that we can start making some revenue

Acceptance Criteria

Given I have a list of plates with prices   
When a customer buys a plate  
Then the plate should be marked as sold

Given I have a list of plates  
When I configure a plate to be sold  
And I search for the plate   
Then the plate should have a status of sold

Given I have a list of plates  
When a plate is sold  
Then a total revenue label should be incremented at the top of the page

\[Advanced\]  
Given i have a list of plates  
When a plate is sold  
Assuming the SalePrice is inc VAT and the Purchase Price is exc VAT  
Then a label with the average profit margin for all sales should be updated   
(if you feel inclined, plot this on a chart)

## 

## User Story 7

As the head of marketing  
I would like to be able to offer money off promotions  
So that we can improve sales

Acceptance Criteria

Given I enter the promo code DISCOUNT  
When a customer searches for a plate  
Then the price should be offered at a £25 discount

\[Advanced\]  
Given i enter the promo code PERCENTOFF  
When a customer searches for a plate  
Then the price should be offered at a 15% discount

## User Story 8

As the head of marketing  
I would like to make sure we do not sell plates under 90% of the sale price  
So that we can control discount codes

Acceptance Criteria

Given I have a plate for sale at £200.00   
When a customer adds the promo code DISCOUNT  
And tries to purchase the plate  
Then the customer should be advised the discount code is not applicable

\[Advanced\]  
If you are having fun and would like to continue, feel free to write code and document it so that we can see what and why you have done it. You are in no means expected to do any more

[image1]: <data:image/png;base64,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>

[image2]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEkAAAAWCAYAAACMq7H+AAAFcklEQVR4Xu1W3U9bZRg/Gl0GA8csFuyADiGUyYBFtslmumggLs7ORcUZnYkXU7dhTHR3YjJDGXTE6I0xcYkMITiYF7osCyxBIVlkDEYRHD14sbELPxJbIPIXPL4fz/t1es5aY7zj4sd5Pn/P8/54T1trbW0N1nFvWPTP6q9HEa+vQ4Jroon0Gqwuvoo4AisEyk+PcdusSc+7x/49p1ss3faO/Yc5RBcp0krixXV4YG3tbyHSC7CaOMxAbd13i2Xru8Wy9d1imXy3WLa+W4zaSqSF52AVQW3lH4C7k82whKA2xZ/zeg32zPPau9OKQ9QIDqNn2uRcipuckjeONZh35cTZxv7Yx3lxt8kDnhxp50FbifRLE6zcIsW3mrjNfIIfdoBlWa7oGzV7ho6IXBAu3dR5wrJHzHFyCXw08IzipE9Sv9jtx3w+z+NuBuc3FcyOdO+Xc0Vf5GwYViZ2wjFWH4C+a2IvtYfaNV0DKdLy/NMkyEFt6Y/Vgh+Jwnu3MFQVon+60ehpw7hl5UH0W51nn1yG+43SF5zhyg08tjcEs8gpeO2zj8h669ntcjeDc6Cc+9XlMDLN+xa7eV8kto/4++HSW3xGc3sjcuyS+xpndmigRJojRHNPIait/AguY4tYf5CTt9YaPfIgFG/WaDy71YH0OXF9Th20sZoyGJGcnNeO+QzuO9hzn84pdqIIbTP6Il27cU4D9jxI7D3w9WFe/8ZnezDvroEmErkVCGrrvhIJY/2lfJnWGqOHxQpz8Fmq8TRoInnNqUGRSuGqo8aOPWyIFL3Ic0okbScttoh9ka4GOUf02BeDUMf23ArfTzl3MX0l0s8NsEJAnwyzT8iYFEnE+gMoUrXquVbJF4rV8eF0UclTp5b3mNPiwwO+UglLmGN1JG/HCnju7RJoD9G6XIgOiVuBnLhTVNyOT3cSkXhfpKtOzhl/P1fuQnHuavp5nbspkeL1JFjPnwQpfNKYECkhYn3FfMjJStXTU0RiORC9UA+tYnnJU6NEcplzpwf5CLovaPlZnrc7N8t5Mx8/xGx/S7kSSdspfq6If4bWBmCog/dFztSo84xtg6PYR/GHtofbbvQpRUrNPA7LCGrrvhRJxHrx2+ZEOdaEYLCF+AdLwCb++KmNLD84Lniq5FLmnO3yS8EqLYAvL5tzxS6JTi4MnZckvuAyOHEnOv/2J3jzEJEzVQbv71/gZ1xVkXFeLw3UTSIHXZ6pRlBb+fJ1E7Fe/LY5UYY1QThFfd8GCD+ZC+GKB1i+rVfw8FeRQvGGYORddfW/G02fK+rsznycFySLh+Q+BifuZNOe62UOkSpM3t5CnmsOyBnps5WvbtL0Y7B8k4Paui9vEsbipzfxIccDzLdjecZSEi8XI4/65hG84gb5D/ohfsN9roglOtS8JK0Z8EGtNof1nOcf0jb26K9UJBo0z4e1VrPfmOulgRJpihIhpso4qH0FVXdB+MMAqxk+xv0PPi+B2z+WEhRDWxWNbUaeEnUgNsf8JjJRAMNiD9wl0YE37ngxJGn/VAn0HlI9jPM8f8US2DP2Hv7usqhIW9V5KHrwdWwqxBmOM+sxYkuRkjcehRSC2tK/bL7fAv6KHFkjbtqMxrHQyX8K/MV8+qHOa3iP+AXthnwYduyyEOWfcdY7PsbHOdQ/j/lf8VcygfkkmTn40v0s9nx7kXk+rLWaCozzemmgRJr0QWqykD0FUlnEqE1jZo3Z44w5816cbrFMnG6xTHOcnM6Yet0mt5i47vCdsWzzbrFMPV55t1imvFssU94R034n7YDURB4kJzYZyBTLlHeLZZt3i/0fPV55CqoL1YeJ9NvcSUiM7oLkTxvXgaB6UF2kSOu4N/4BJhW9FXEsQDcAAAAASUVORK5CYII=>

[image3]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEgAAAAVCAYAAADl/ahuAAAEqElEQVR4Xu2Yz29UVRTHZ4GkNG0zkxmGgqU0tlIb7VgnFCSkblp/RVxUcSFdGELSKggJE91M44IKVuMGExbzFpRqSfyRSGLUKYHERgIttBQ7wEzrQjYms5hf2r/g+N6999z77o+5fS6MGxbfvHs+55zvvffkzUwyofX1dXik+grh4u/KfaitHYba6ttEVfasrbGnwnAdlHEFZX6f/4BpZ/Uzbw5rI/KA/vr9CNQKh4iqTBibGMZBmc3LxGxeJmbzMjGbFzJpQN7EaoU3mIaZMDYxjIMym5eJ2bxMzOZlYjYvyvQB5Q8SVR9QYWxiGAdlNi8Ts3mZmM3LxGxeyLQBVR+8SnX/FSqMTezOIDycZ1oYkmtYncd5zbLPC3sXfew2ZcWcsp+roufjqrhCY9xT8ifxy6S3yBj38jP1Pv47KUwf0L1BogoTxipLhEIQMqj/w72k5mhMz5H8yb2kv/BZnLEd1P9mHxxlNeOX5P0q9wbAeYnm0pe8eI/mK9Tp9op69Kq6PWmSj4NzTfXX74hMH1DuBaJKboAIY5k9zw7TJNhiAkKxRnjz+NMkxgOLvudgojsEuxLthBU+jfGaohvPfdDE4/EZeb9Kbp8Y0AwycTZvPXeK9seP95Hz8QExr2qujw0oBs5V1V/28jPDgA4IrfjWEutnl4lKrHhX9IgBIdsPq98nuc/qZJTXXHHjL/aLt2B8Rt9PXFjs4VeKvbET3xnqSU0CxtmZnat+b8VPYfqAfuuHiiKd9bDLROrWxdll411RSE/2SHnvWZiM8IEMfJTga3Kpr9T9+sB5kebSvpzYL8l7KRP1qfNJeHg9CX9c3w0pdmbniulOZqYNqHI3GUDd7EBhQ45q+rXHpEsPvbUTvvnhWZ4vfBIW+QOPS7XpL1W/XjEgLedqtkM5j6jXFQZn1uBRR/qAlnuJysvPEGEsM3yDWkicGfIfgDLsnT27DUZ6N/N8//tdhOfPtlA2jG9SBEaG2RCm5f3Ky0+Bw/ZITyPDs3XB6W4vtwlSDrIe5Uzy+Zys6i/uqTJ9QHe6oexpicld66yTbdZM4q8PN8LAvi2MhfVeErfDKdbjsfyZZlr/bjt59qY6IDvGhnBR2W+pk184PSWf7fKRTbT/ZAftIfLVoxdnzZDJqv5YozN9QEtPBtATbBhNPraTsSiNf+2Auc9bpTz9FaE9hTPsV2uM9h073wWzOKApfT/+Bim5Y8QzBOcub1SPrAmcn1X/+tIGVF7cFUh0GAa9vpXk8Uta0+4Iyec/bqTxWCv35G/QlLpfG2QGTbk27rtxPbJGyPyk+teXPqDbO5i2Q/nWdl+ssGwUTh8U3y2ezn2LeVdzW+HiKH7smNoauFd+ooGy0Rhn2VF2qQvqftvEhS+IPX45QX8I4u9ElbO2+upV1gCZHxnDs/p7FaYP6FYcygtxKDF563oM46CMxP+CaV4bMO4VkNm8kGkDKs1HmMJMGJsYxkGZzcvEbF4mZvMyMZsXZdKAvP+DyvMtXCXf2sRwHZTV8/m/mO2s5LmgDMj7RzF/bQ+Ubm6B0o0GKm+NUhnGQZnNy8RsXiZm8zIxi5c3hz9X3pMH9Ehm/QPUyVAFALHA9AAAAABJRU5ErkJggg==>

[image4]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEcAAAAWCAYAAACSYoFNAAAEUklEQVR4Xu1Y30+TVxj+0M04UZy2gqIUsy2tbDD2A4Zoii5s3KxuGrlxLCZmCWi4cUu8ELLEoJtjiYmJF6PJ3HTTRK62EVOqDn/M6WRaZbi2LIHFxKgJLSh/wbvv/HrP+c53SmtK9EIuHs55nvO+73nfp19bUmtqagpmYYZF/jxK34ZHd76ByZGWZx4P/91BvUBzJke288OPZzGyDYgX5IFh5iSbnUhoPBdN7E1appxctSdRWzsnTxA1ZyL+IUzGP0IQrmo6N2mCmzSdm7SnXVuPIU8PM+efD2yEtFXf56NlO89Hy3aeq+acX5pzuyl/DG8Ay7Ig/JuqvQ+/7LCobrW9gxpZk90lTG+qQi3cxGI7T8o4xGA9i1fh80L7kSDG/Hfc74pp3lsP9/VeVej3cE2aM9xoi+/Z4CvhqoZ8Gm04yMw5p2qN0LWGN1pfATdFjo1kdzEfoBTrfMoH6jyh126EaPsi1+AMi7GfWtcZwTxoPkQMzNS34M75pTl/N8wA1jJzzjp1tdHPj0k9+bUX9ftabOdPeu0G6NxcBJZ3NUQNtVn+OtcdE5depeb4N/hd9bIBzUkPrYUJBYSrms5NWnrobWbOGbdmlb3A1pAf85IHl+JwXb12/JlXkHf8qNc23cdeDIIk5TV0Hzrwpitv+r7NMdKcW7UzgDeYOVFFGwxQLfjF63yQUjxLHFyCw5HzscPibUbM0Ws7Mfb7WzZETQt1wcvXF8PYDXfe40CaE6uG9M1qXFNkVbVYdi0Ve4021hORWqxrsa154dSlagjyxkVO4ityJt9yKjqO6bWd9xUUKPHvlsl+LgegWKsV2uOHe9P2LfeqpphTNQOooM2EI4JXws/b7QbrVkHM5h0e1uw9Hp/4sogNsEU+QZ9sEebotZ1oqFsIwap5mOc4H/DDqT0exaDnYGt3hatGNkhzbgRsrFFAuKrp3KS9zM3hWl8phEhz9udNsG4B1JaxZtl5wDaHf/vs9OEg/Tu5OT/otQV3amhOppi+FTxmmaFWJs40NCf110uQvi5BuKrpvL/9efr4hvb5ZMxF1kjPacYvtM+FAnz1JKp2r6S1EgcKmdZWirX727g537vvD9C3UiGE+6SG5lBeTvct+8sxb/TIizxmacZZdC40ac5gOS2OGPQ5Ncp9yEcPZfqfw4LzPK5no0XNOdy7CkYHymD0Vy+0kJiNHlorvn8By2ldjrUjwpyj/E7l/s+87rsERIyuIzYtyziLi3MNzRm/tgJSCghXNZ1TLeoBn9bEvnAJxokPxrtKzsCuObY2h8bEu+azvFYP5kRaWc7e78z3t1TOddxXEyqCSFSJMfT0be/yx5/NhnxyrhXbKFFAuKrp3KQJbtJ0btKedm1njDTnzyVOXNV4LprYm7RMOblqT6K2dq58lVdC6spCGL9SiKu+z0fLdp6Plu08V02fn/waSM25O7QL4udqYPyP+c88Hlwuol7gL4GzMON/ljzqep5AbEQAAAAASUVORK5CYII=>