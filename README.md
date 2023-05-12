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

<table>
    <tr>
        <th>Left Input</th><td><code>{"input": "This is some test data."}</code></td>
    </tr>
    <tr>
        <th>Right Input</th><td><code>{"input": "This is some test data."}</code></td>
    </tr>
    <tr>
        <th>Output</th><td><code>{"diff":"LeqR"}</code></td>
    </tr>
</table>



### 1st stream longer than 2nd stream

<table>
    <tr>
        <th>Left Input</th><td><code>{"input": "This is longe<span style="color: red;">r test data.</span>"}</code></td>
    </tr>
    <tr>
        <th>Right Input</th><td><code>{"input": "Shorter data."}</code></td>
    </tr>
    <tr>
        <th>Output</th><td><code>{"diff":"LgtR"}</code></td>
    </tr>
</table>



### 1st stream shorter than 2nd stream

<table>
    <tr>
        <th>Left Input</th><td><code>{"input": "This is shorter data."}</code></td>
    </tr>
    <tr>
        <th>Right Input</th><td><code>{"input": "This is a very long s<span style="color: red;">tringggggg.</span>"}</code></td>
    </tr>
    <tr>
        <th>Output</th><td><code>{"diff":"LltR"}</code></td>
    </tr>
</table>



### Streams with same length, but there are differences

<table>
    <tr>
        <th>Left Input</th><td><code>{"input": "Th<span style="color: red;">is</span> is <span style="color: red;">some</span> test data."}</code></td>
    </tr>
    <tr>
        <th>Right Input</th><td><code>{"input": "Th<span style="color: red;">at</span> is <span style="color: red;">also</span> test data."}</code></td>
    </tr>
    <tr>
        <th>Output</th>
        <td>
            <pre>
            <code>
                {
                    "diff": "LdiR",
                    "diffSections": [
                        {"offset": 2, "length": 2},
                        {"offset": 8, "length": 4}
                    ]
                }
            </code>
            </pre>
        </td>
    </tr>
</table>
