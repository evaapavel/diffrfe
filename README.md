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
   - Plain: `{"result":"LeqR"}`
2. 1st stream (the "left" one) is longer than 2nd (the "right" one).
   - Plain: `{"result":"LgtR"}`
3. 1st stream (the "left" one) is shorter than 2nd (the "right" one).
   - Plain: `{"result":"LltR"}`
4. The streams are of the same length, but they differ in some characters.
   - Plain: `{"result":"LdiR", "diffSections":[{"offset":2, "length":4}, {"offset":10, "length":1}, {"offset":56, "length":12}]}`


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
        <th>Output</th><td><code>{"result":"LeqR"}</code></td>
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
        <th>Output</th><td><code>{"result":"LgtR"}</code></td>
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
        <th>Output</th><td><code>{"result":"LltR"}</code></td>
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
    "result": "LdiR",
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



## Architecture and classes


### Business and data-transfer objects

<dl>
    <dt>StreamInput</dt>
    <dd>Represents one character stream to be compared (diffed).</dd>
    <dt>DiffOutput</dt>
    <dd>Represents the result of the diff operation.</dd>
    <dt>Diff</dt>
    <dd>Encapsulates input data and the result. Instances of this type are stored in the in-memory "database".</dd>
    <dt>DiffResult</dt>
    <dd>Enum describing the possible results of the diff operation (LeqR, LgtR, LltR and LdiR).</dd>
</dl>


### Data layer

<dl>
    <dt>IDiffRepo</dt>
    <dd>Interface defining core methods of the data layer.</dd>
    <dt>DiffRepo</dt>
    <dd>Class implementing the <code>IDiffRepo</code> interface. It behaves like a singleton. All data resides in memory only.</dd>
</dl>


### Application layer

<dl>
    <dt>IDiffService</dt>
    <dd>
        Interface forcing a contract of what the diff service is capable of.
        It exposes CRUD methods for character stream data as well as the "diff" method (string comparison) itself.
    </dd>
    <dt>DiffService</dt>
    <dd>Implements the <code>IDiffService</code> interface.</dd>
</dl>


### Front layer (instead of Presentation)

<dl>
    <dt>DiffController</dt>
    <dd>An API controller that implements all the necessary end-points.</dd>
</dl>

