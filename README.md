# Diff RFE

A diff service for RFE.


## Endpoints

- GET `<host>/v1/diff/get-id`
  - Generates a new unique ID in order to use the service.

- POST `<host>/v1/diff/<ID>/left`
  - Puts 1st version of the character stream to compare. (See below for the data structure within the request body.)

- POST `<host>/v1/diff/<ID>/right`
  - Puts 2nd version of the character stream to compare. (See below for the data structure within the request body.)

- GET `<host>/v1/diff/<ID>`
  - Gets the result of the diff operation applied to the two versions of the input character stream.


## Data examples

### Input data

Base64: `eyJpbnB1dCI6InRlc3RWYWx1ZSJ9`
Plain:  `{"input":"testValue"}`

### Output data

1. The streams are identical.
   - Plain: `{"diff":"LeqR"}`
2. 1st stream (the "left" one) is longer than 2nd (the "right" one).
   - Plain: `{"diff":"LgtR"}`
3. 1st stream (the "left" one) is shorter than 2nd (the "right" one).
   - Plain: `{"diff":"LltR"}`
4. The streams are of the same length, but they differ in some characters.
   - Plain: `{"diff":"LdiR", "diffSections":[{"offset":2, "length":4}, {"offset":10, "length":1}, {"offset":56, "length":12}]}`


## Case examples

### Identical (equal) streams (same size, same characters)

| Left Input | Right Input | Output |
|:-----------|:------------|:-------|
| `{"input": "This is some test data."}` | `{"input": "This is some test data."}` | `{"diff":"LeqR"}` |



### 1st stream longer than 2nd stream

| Left Input | Right Input | Output |
|:-----------|:------------|:-------|
| `{"input": "This is longer test data."}` | `{"input": "Shorter data."}` | `{"diff":"LgtR"}` |



### 1st stream shorter than 2nd stream

| Left Input | Right Input | Output |
|:-----------|:------------|:-------|
| `{"input": "This is shorter data."}` | `{"input": "This is a very long stringggggg."}` | `{"diff":"LltR"}` |



### Streams with same length, but there are differences

| Left Input | Right Input | Output |
|:-----------|:------------|:-------|
| `{"input": "This is some test data."}` | `{"input": "That is also test data."}` | `{"diff":"LdiR", "diffSections":[{"offset":2, "length":2}, {"offset":8, "length":4}]}` |
