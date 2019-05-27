import sys
from selenium import webdriver
from selenium.webdriver.common.keys import Keys

newToken = sys.argv[0]

driver = webdriver.Firefox()
driver.get("https://api.thetvdb.com/swagger")

token = driver.find_element_by_id("input_apiKey")
token.send_keys(newToken);

driver.find_element_by_id("add-token").click()