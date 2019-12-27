<!--
 /**
  *
  * input 禁用状态清除值按钮
  *
  * author: lanbo
  *
  * version: 0.1
  *
  * description: 用于在 input 在 disabled 或 readonly 状态下清除值，如果非 disabled 和非 readonly 状态下则无效，使用 input 正常的 clearable 属性
  *
  * props:
  *
  * events:
  *
  * changelog
  *     v0.1 2019-01-04 by lanbo | 基本功能
  *
  */
-->
<template>
<div class="ex-input-suffix-clear" v-show="visible">
    <i class="el-input__icon el-icon-circle-close el-input__clear" @click="handleClearClick"></i>
</div>
</template>

<script>
export default {
    name: 'ExInputSuffixClear',
    props: {
    },
    data() {
        return {
            visible: false,
            unwatch: null
        };
    },
    methods: {
        handleClearClick() {
            if (this.$parent && this.$parent.clear && this._.isFunction(this.$parent.clear)) {
                this.$parent.clear();
            }
        }
    },
    created() {
        this.unwatch = this.$watch(() => {
            if ((this.$parent.disabled || this.$parent.readonly) && (this.$parent.value != null && this.$parent.value !== '') && (this.$parent.hovering || this.$parent.focused)) {
                return true;
            }

            return false;
        }, (val) => {
            this.visible = val;
        });
    },
    beforeDestroy() {
        if (this.unwatch) {
            this.unwatch();
        }
    }
};
</script>

<style scoped lang="less">
.ex-input-suffix-clear {
    .el-input__clear {
        cursor: pointer;
    }
}
</style>
