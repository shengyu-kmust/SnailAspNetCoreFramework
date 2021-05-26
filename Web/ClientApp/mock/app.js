/**
 * 此js在测试阶段，请忽略
 */
const express = require('express')
const { mocks } = require('./index')

const app = express()
const port = 3000

mocks.forEach(item => {
  app[item.type](item.url, (req, res) => {
    const ret = item.response(req)
    res.json(ret)
  })
})

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`)
})
