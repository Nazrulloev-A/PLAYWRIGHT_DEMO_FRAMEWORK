Feature: TrioLogin

    @SmokeSuite @LoginTest
    Scenario Outline: User is able to Login with different User role id and verify the page
        Given user navigates to Test home page
        When user logs in using username and password for <userRole>
        And user successfully logged out

        Examples:
          | userRole |
          | Staff    |
#          | Client   |
#          | Agency   |

#    @SmokeSuite @LoginDeniedTest
#    Scenario Outline:API Service-Only User - Restricted Access to Web Features.
#        Given user navigates to Trio home page
#        When user logs in as api only user
#        Then user should only have limit access view
#
#
#    @SmokeSuite @LoginDeniedTest
#    Scenario Outline:API Service-Only - Redirection UI Access for User with Jobs Permission.
#        Given user navigates to Trio home page
#        When user logs in as api only user
#        Then user with access to Jobs should not be able to get to that part of the UI.