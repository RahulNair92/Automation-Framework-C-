Feature: Fileupload
![Bond adapter](https://www.leicabiosystems.com/themes/contrib/lbs-drupal-theme/logo.svg)
	Test file upload feature

Scenario: verify jwt token for apis
	Given I get JWT authentication of User with following detailss
		| Email             | Password |
		| eve.holt@reqres.in | cityslicka  |

	Then I get JWT authentication of User with following detailss
		| Email             | Password |
		| eve.holt@reqres.in | cityslicka  |
	
	When I get JWT authentication of User with following detailss
		| Email             | Password |
		| eve.holt@reqres.in | cityslicka  |
	

	
	


