/**
fields:{
  name:'',
  type:'',//支持：string,int,datetime,date,select,multiSelect
  label:'',
  keyValues:[],//
}
和snailForm一样，只是样式有点变化

 */
<template>
  <div>
    <el-form :ref="validateFormName" :rules="rules" :model="formData" style="margin:10px" label-position="left" :inline="true">
      <template v-for="(item,index) in fields">
        <el-form-item :key="index" v-bind="item" :prop="item.name">
          <!-- 输入 -->
          <el-input v-if="item.type==='string'" v-model="formData[item.name]" />
          <!-- 时间 -->
          <el-date-picker
            v-if="item.type==='datetime'"
            v-model="formData[item.name]"
            style="width:100%"
            type="datetime"
            v-bind="item"
          />
          <!-- 日期 -->
          <el-date-picker
            v-if="item.type==='date'"
            v-model="formData[item.name]"
            style="width:100%"
            type="date"
            v-bind="item"
          />
          <!-- 单选 -->
          <el-select
            v-if="item.type==='select'"
            v-model="formData[item.name]"
            style="width:100%"
            v-bind="item"
          >
            <el-option
              v-for="keyValue in item.keyValues"
              :key="keyValue[item.key || 'key']"
              :label="keyValue[item.key || 'value']"
              :value="keyValue[item.key || 'key']"
            ></el-option>
          </el-select>
          <!-- 多选 -->
          <el-select
            v-if="item.type==='multiSelect'"
            v-model="formData[item.name]"
            multiple
            style="width:100%"
            v-bind="item"
          >
            <el-option
              v-for="keyValue in item.keyValues"
              :key="keyValue[item.key || 'key']"
              :label="keyValue[item.key || 'value']"
              :value="keyValue[item.key || 'key']"
            ></el-option>
          </el-select>
          <el-input-number v-if="item.type==='int'" v-model="formData[item.name]" v-bind="item"></el-input-number>
        </el-form-item>
      </template>
      <el-button type="primary" @click="search">查询</el-button>
      <el-button type="primary" @click="resetSearch">重置</el-button>
      <slot name="searchAfterSlot">
      </slot>
    </el-form>

  </div>
</template>

<script>
import { FormBaseMixin } from './formBase.js'
export default {
  name: 'SnailSearchForm',
  mixins: [FormBaseMixin],
  methods: {
    search() {
      this.$emit('search', this.formData)
    }
  }
}
</script>
