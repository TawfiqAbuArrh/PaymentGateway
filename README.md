# PaymentGateway
Payment Gateway task

To sucre the API more, First of all use HTTPS.
Second, i add login API, and it return Token to user when he successfully logged in, so he can use it in other APIs.
This will gurantee that all transactions will be only made from authroized person.

<h2>Note, Admin User can't make any normal operation that other accounts can make.</h2>

For transaction table, you can separate the transaction in differnet tables (Refund Table, Withdraw Table, etc.), but for the sake of simplicity and to harry up,
I just add new record with the same name and the negative amount in tranaction table and the same ID but with a new suffix in the begging.

*For refund and withdraw APIs, I assumed the logic is the transaction must be buy to make refund or withdraw, i can't be withdraw in refund and vice versa*

User must be approved from admin to use APIs

Upload PDF API, to upload pdf file, the body must be form-data, and you must select File type.
GetPDF API, it will return to you a complete PDF file. you must save the response if you are using postman to see the return PDF file.

Please see the Postman API Collection and import it into your client and start using the API.

A Diagram from my Database:

![Image](/PaymentGateway.png?raw=true "Database Diagram")
